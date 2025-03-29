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
        Debug.Log("�鿣�� ����Ʈ �ý��� ������ ȣ���մϴ�.");
        PostListGet(PostType.Admin);
    }

    public void PostListGet(PostType postType)
    {
        Backend.UPost.GetPostList(postType, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"���� �ҷ����� ��, ������ �߻��Ͽ����ϴ� : {callback}");
            }

            else
            {
                Debug.Log($"���� �ҷ����⿡ �����Ͽ����ϴ� : {callback}");


                try
                {
                    LitJson.JsonData jsonData = callback.GetFlattenJSON()["postList"];

                    if(jsonData.Count <= 0)
                    {
                        Debug.LogWarning("�������� ����ֽ��ϴ�.");
                        return;
                    }

                    postList.Clear();

                    //���� ���� ������ ��� ���� ���� �ҷ�����

                    for(int i = 0; i < jsonData.Count; i++)
                    {
                        PostData post = new PostData();
                        post.Title = jsonData[i]["title"].ToString();
                        post.content = jsonData[i]["content"].ToString();
                        post.inDate = jsonData[i]["inDate"].ToString();
                        post.expiration_Date = jsonData[i]["expirationDate"].ToString();

                        // ���� �߼۵� ��� ������ ���� �ҷ�����

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
                                Debug.LogWarning($"���� �������� �ʴ� ��Ʈ ���� �Դϴ� : {jsonData[i]["chartName"].ToString()}");
                                post.isCanReceive = false;
                            }
                        }

                        postList.Add(post);

                    }

                    // ���� ������ ���� ���� ��� ���

                    for(int i = 0; i< postList.Count; i++)
                    {
                        Debug.Log($"{i}��° ����\n {postList[i].ToString()}");
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
