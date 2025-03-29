using BackEnd;
using GoogleMobileAds.Ump.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackEnd_PostSystem : MonoBehaviour
{
    private List<PostData> postList = new List<PostData>();

    private void Start()
    {
        Debug.Log("백엔드 포스트 시스템 구문을 호출합니다.");
        PostListGet(PostType.Admin);
    }

    public void PostListGet(PostType postType)
    {
        Backend.UPost.GetPostList(postType, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"우편 불러오기 중, 에러가 발생하였습니다 : {callback}");
            }

            else
            {
                Debug.Log($"우편 불러오기에 성공하였습니다 : {callback}");


                try
                {
                    LitJson.JsonData jsonData = callback.GetFlattenJSON()["postList"];

                    if(jsonData.Count <= 0)
                    {
                        Debug.LogWarning("우편함이 비어있습니다.");
                        return;
                    }

                    postList.Clear();

                    //현재 저장 가능한 모든 우편 정보 불러오기

                    for(int i = 0; i < jsonData.Count; i++)
                    {
                        PostData post = new PostData();
                        post.Title = jsonData[i]["title"].ToString();
                        post.content = jsonData[i]["content"].ToString();
                        post.inDate = jsonData[i]["inDate"].ToString();
                        post.expiration_Date = jsonData[i]["expirationDate"].ToString();

                        // 우편에 발송된 모든 아이템 정보 불러오기

                        foreach(LitJson.JsonData itemJson in jsonData[i]["items"])

                        {
                            if (itemJson.ContainsKey("chartName") &&
                                itemJson["chartName"].ToString() == Base_Manager.instance.GOODS_CHART_NAME)
                            {
                                
                                string itemName = itemJson["item"]["ItemName"].ToString();
                                int itemCount = int.Parse(itemJson["itemCount"].ToString());

                                if (post.post_reward.ContainsKey(itemName))
                                {
                                    post.post_reward[itemName] += itemCount;
                                }

                                else
                                {
                                    post.post_reward.Add(itemName, itemCount); 
                                }

                                post.isCanReceive = true;
                            }
                            else
                            {
                                Debug.LogWarning($"아직 지원하지 않는 차트 정보 입니다 : {jsonData[i]["chartName"].ToString()}");
                                post.isCanReceive = false;
                            }
                        }

                        postList.Add(post);

                    }

                    // 저장 가능한 우편 정보 모두 출력

                    for(int i = 0; i< postList.Count; i++)
                    {
                        Debug.Log($"{i}번째 우편\n {postList[i].ToString()}");
                    }
                }
                catch(System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
        });
    }


}
