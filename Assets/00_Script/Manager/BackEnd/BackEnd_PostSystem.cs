using BackEnd;
using GoogleMobileAds.Ump.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BackEnd_PostSystem : MonoBehaviour
{
    [System.Serializable]
    public class PostEvent : UnityEvent<List<PostData>> { }
    public PostEvent onGetPostListEvent = new PostEvent();


    private List<PostData> postList = new List<PostData>();

    private void Start()
    {       
        PostListGet(PostType.Admin);
        DontDestroyOnLoad(this);
    }

    public void All_ReCeive_Button()
    {
        PostReceiveAll(PostType.Admin);
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

                    // ���� ����Ʈ �ҷ����Ⱑ �Ϸ�Ǿ��� ��, �̺�Ʈ �޼ҵ� ȣ��
                    onGetPostListEvent?.Invoke(postList);

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
    public void PostReceive(PostType postType, int index)
    {
        if(postList.Count <= 0)
        {
            Debug.LogWarning("���� �� �ִ� ������ �������� �ʽ��ϴ�. Ȥ�� ���� ����Ʈ �ҷ����⸦ ���� ȣ�����ּ���.");
            return;
        }

        if(index >= postList.Count)
        {
            Debug.LogError($"�ش� ������ �������� �ʽ��ϴ�. : ��û : index : {index} / ���� �ִ� ���� :{postList.Count}");
            return;
        }

        Debug.Log($"{postType.ToString()}�� {postList[index].inDate} ���� ������ ��û�մϴ�.");


        Backend.UPost.ReceivePostItem(postType, postList[index].inDate, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"{postType.ToString()}�� {postList[index].inDate} ���� ���ɿ� ������ �߻��Ͽ����ϴ�. : {callback}");
            }

            Debug.Log($"{postType.ToString()}�� {postList[index].inDate} ���� ���ɿ� �����Ͽ����ϴ�. : {callback}");
            postList.RemoveAt(index);

            if (callback.GetFlattenJSON()["postItems"].Count > 0)
            {
                SavePostToLocal(callback.GetFlattenJSON()["postItems"]);

                _=Base_Manager.BACKEND.WriteData();
            }

            else
            {
                Debug.LogWarning("���� ������ ������ �������� �ʽ��ϴ�.");
            }
        });


    }
    public void PostReceive(PostType postType, string inDate)
    {
        PostReceive(postType, postList.FindIndex(item => item.inDate.Equals(inDate)));
    }
    public void PostReceiveAll(PostType postType)
    {
        if(postList.Count <= 0)
        {
            Debug.LogWarning("���� �� �ִ� ������ �������� �ʽ��ϴ�. Ȥ�� ���� ����Ʈ �ҷ����⸦ ���� ȣ�����ּ���.");
            return;
        }

        Debug.Log($"{postType.ToString()} ���� ��ü ������ ��û�մϴ�.");

        Backend.UPost.ReceivePostItemAll(postType, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError($"{postType.ToString()} ���� ��ü ���� �� ������ �߻��߽��ϴ�. : {callback}");
                return;
            }

            Debug.Log($"���� ��ü ���ɿ� �����Ͽ����ϴ�. : {callback}");

            postList.Clear(); // ��� ������ �����Ͽ���, postList�� �ʱ�ȭ�Ѵ�.

            foreach(LitJson.JsonData postItemsJson in callback.GetFlattenJSON()["postItems"])
            {
                SavePostToLocal(postItemsJson);
            }

            _ = Base_Manager.BACKEND.WriteData();

        });
        
    }
    public void SavePostToLocal(LitJson.JsonData item)
    {
        try
        {
            foreach(LitJson.JsonData itemJson in item)
            {
                // ��Ʈ ���� �̸�(*.xlsx) �� Backend Console �� ����� ��Ʈ �̸�
                string chartFileName = itemJson["item"]["chartFileName"].ToString();
                string chartName = itemJson["chartName"].ToString();

                //GoodsChart.xlsx �� ����� ù��° �� �̸�

                int ItemId = int.Parse(itemJson["item"]["ItemId"].ToString());
                string itemName = itemJson["item"]["ItemName"].ToString();
                string itemInfo = itemJson["item"]["ItemInfo"].ToString();

                // ���� �߼� �� �� , �ۼ��ϴ� ������ ����

                int itemCount = int.Parse(itemJson["itemCount"].ToString());

                // �������� ���� ��ȭ�� ���� �� �����Ϳ� ����
                // ���� ��ȭ�� �����, ���⸦ ������Ʈ �ؾ��մϴ�!
                if (chartName.Equals(Base_Manager.instance.GOODS_CHART_NAME))
                {
                    if (itemName.Equals("Dia"))
                    {
                        Data_Manager.Main_Players_Data.DiaMond += itemCount;
                        Base_Manager.BACKEND.Log_Get_Dia("Post");
                    }
                }

                Debug.Log($"{chartName} - {chartFileName}");
                Debug.Log($"{ItemId} {itemName} : {itemInfo}, ȹ�� ���� : {itemCount}");
                Debug.Log($"�������� �����߽��ϴ�. : {itemName} - {itemCount}��");

            }
        }

        catch(System.Exception e)
        {
            Debug.LogError(e);
        }
    }


}
