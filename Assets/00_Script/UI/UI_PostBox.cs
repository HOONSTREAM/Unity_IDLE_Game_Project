using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_PostBox : UI_Base
{ 
    [SerializeField]
    private GameObject PostParts_UI_PreFab;
    [SerializeField]
    private Transform Contents; // 우편 UI가 배치되는 ScrollView의 Content
    [SerializeField]
    private GameObject Empty_Text_System; // "우편함이 비어있습니다." 의 텍스트 오브젝트

    private List<GameObject> PostList;

    private void Awake()
    {
        PostList = new List<GameObject>();
    }

    public override void DisableOBJ()
    {
        DestroyPostAll();
        base.DisableOBJ();
    }

    public void SpawnPostAll(List<PostData> PostDataList)
    {
        for(int i = 0; i<PostDataList.Count; i++)
        {
            GameObject clone = Instantiate(PostParts_UI_PreFab, Contents);
            clone.GetComponent<UI_Post_Parts>().SetUp(this.GetComponent<BackEnd_PostSystem>(),this,PostDataList[i]);
            PostList.Add(clone);
        }

        Empty_Text_System.gameObject.SetActive(false);
    }

    public void DestroyPostAll()
    {
        foreach( GameObject post in PostList)
        {
            if(post != null)
            {
                Destroy(post);
            }
        }

        PostList.Clear();

        Empty_Text_System.gameObject.SetActive(true);
    }

    public void DestroyPost(GameObject post)
    {
        Destroy(post);
        PostList.Remove(post);

        if(PostList.Count == 0)
        {
            Empty_Text_System.gameObject.SetActive(true);
        }
    }
}
