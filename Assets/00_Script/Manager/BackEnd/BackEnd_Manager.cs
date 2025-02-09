using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using BackEnd;
using UnityEngine;

public partial class BackEnd_Manager : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

        var bro = Backend.Initialize(); // �ڳ� �ʱ�ȭ

        // �ڳ� �ʱ�ȭ�� ���� ���䰪
        if (bro.IsSuccess())
        {
            Debug.Log("�ڳ� ���� �ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 204 Success
        }
        else
        {
            Debug.LogError("�ڳ� ���� �ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 400�� ���� �߻� 
        }      
    }

    private void Start()
    {
        User_Data_Initialize();
        Loading_Scene.instance.LoadingMain();
    }

    private void User_Data_Initialize()
    {
        //CustomSignUp("user1", "1234"); // [�߰�] �ڳ� ȸ������ �Լ�
        CustomLogin("user1", "1234");

        ReadData(); // �����͸� �ʱ�ȭ �մϴ�

        WriteData(); //������ ����� �����͸� ������Ʈ�մϴ�.

        Debug.Log("�׽�Ʈ�� �����մϴ�.");
    }

    private void OnDestroy()
    {
        if (Base_Manager.Get_MainGame_Start)
        {
            Debug.Log("������ ���������� �����ϰ�, �����͸� �����մϴ�.");
            WriteData();
        }
    }

}
