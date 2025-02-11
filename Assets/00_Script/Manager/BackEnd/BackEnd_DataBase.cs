using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using BackEnd;
using BackEnd.BackndNewtonsoft.Json.Linq;
using LitJson;

public partial class BackEnd_Manager : MonoBehaviour
{
    /// <summary>
    /// 유저 데이터를 저장합니다. BackendReturnObject는 서버와 통신한 결과값을 의미합니다.
    /// </summary>
    public void WriteData()
    {
        Debug.Log("WriteData 메서드 호출, 데이터를 기록합니다.");
        #region DEFAULT DATA

        if (Data_Manager.Main_Players_Data == null)
        {
            Debug.LogError("데이터가 존재하지 않습니다. Initialize 통해 데이터를 생성해주세요.");
            return;
        }
        
        Param param = new Param();

        param.Add("NICK_NAME", Data_Manager.Main_Players_Data.Nick_Name);
        param.Add("ATK", Data_Manager.Main_Players_Data.ATK);
        param.Add("HP", Data_Manager.Main_Players_Data.HP);
        param.Add("PLAYER_MONEY", Data_Manager.Main_Players_Data.Player_Money);
        param.Add("DIAMOND", Data_Manager.Main_Players_Data.DiaMond);
        param.Add("PLAYER_LEVEL", Data_Manager.Main_Players_Data.Player_Level);
        param.Add("PLAYER_EXP", Data_Manager.Main_Players_Data.EXP);
        param.Add("PLAYER_STAGE", Data_Manager.Main_Players_Data.Player_Stage);
        param.Add("EXP_UPGRADE_COUNT", Data_Manager.Main_Players_Data.EXP_Upgrade_Count);
        param.Add("BUFF_TIMER", Data_Manager.Main_Players_Data.Buff_Timers);
        param.Add("SPEED", Data_Manager.Main_Players_Data.buff_x2_speed);
        param.Add("BUFF_LEVEL", Data_Manager.Main_Players_Data.Buff_Level);
        param.Add("BUFF_LEVEL_COUNT", Data_Manager.Main_Players_Data.Buff_Level_Count);
        param.Add("QUEST_COUNT", Data_Manager.Main_Players_Data.Quest_Count);
        param.Add("HERO_SUMMON_COUNT", Data_Manager.Main_Players_Data.Hero_Summon_Count);
        param.Add("HERO_PICKUP_COUNT", Data_Manager.Main_Players_Data.Hero_Pickup_Count);
        param.Add("RELIC_SUMMON_COUNT", Data_Manager.Main_Players_Data.Relic_Summon_Count);
        param.Add("RELIC_PICKUP_COUNT", Data_Manager.Main_Players_Data.Relic_Pickup_Count);
        param.Add("START_DATE", Data_Manager.Main_Players_Data.StartDate);
        param.Add("END_DATE", DateTime.Now.ToString());
        param.Add("DAILY_ENTER_KEY", Data_Manager.Main_Players_Data.Daily_Enter_Key);
        param.Add("USER_KEY_ASSETS", Data_Manager.Main_Players_Data.User_Key_Assets);
        param.Add("DUNGEON_CLEAR_LEVEL", Data_Manager.Main_Players_Data.Dungeon_Clear_Level);
        
        Debug.Log("유저 기본 데이터를 수정합니다");

        var bro = Backend.GameData.Update("USER", new Where(), param);

        if (bro.IsSuccess())
        {
            Debug.Log("유저 기본 데이터 수정에 성공했습니다. : " + bro);
        }
        else
        {
            Debug.LogError("유저 기본 데이터 수정에 실패했습니다. : " + bro);
        }

        
        #endregion

        #region CHARACTER DATA

        Param character_param = new Param();
        string char_Json_Data = JsonConvert.SerializeObject(Base_Manager.Data.character_Holder);
        character_param.Add("character", char_Json_Data);

        Debug.Log("영웅 보유 데이터를 수정합니다");

        var character_bro = Backend.GameData.Update("CHARACTER", new Where(), character_param);

        if (character_bro.IsSuccess())
        {
            Debug.Log("영웅 보유 데이터 수정에 성공했습니다. : " + character_bro);
        }
        else
        {
            Debug.LogError("영웅 보유 데이터 수정에 실패했습니다. : " + character_bro);
        }
        #endregion

        #region ITEM_DATA
        Param item_param = new Param();
        string Json_item_Data = JsonConvert.SerializeObject(Base_Manager.Data.Item_Holder);
        item_param.Add("Item", Json_item_Data);

        Debug.Log("인벤토리 데이터를 수정합니다");

        var item_bro = Backend.GameData.Update("ITEM", new Where(), item_param);

        if (item_bro.IsSuccess())
        {
            Debug.Log("유저 인벤토리 데이터 수정에 성공했습니다. : " + item_bro);
        }
        else
        {
            Debug.LogError("유저 인벤토리 데이터 수정에 실패했습니다. : " + item_bro);
        }
        #endregion

        #region SMELT_DATA
        Param smelt_param = new Param();
        string Json_Smelt_Data = JsonConvert.SerializeObject(Base_Manager.Data.User_Main_Data_Smelt_Array); 
        smelt_param.Add("Smelt", Json_Smelt_Data);
     
        Debug.Log("유저 각인 데이터를 수정합니다");

        var smelt_bro = Backend.GameData.Update("SMELT", new Where(), smelt_param);

        if (smelt_bro.IsSuccess())
        {
            Debug.Log("유저 영웅 각인 데이터 수정에 성공했습니다. : " + smelt_bro);
        }
        else
        {
            Debug.LogError("유저 영웅 각인 데이터 수정에 실패했습니다. : " + smelt_bro);
        }
        #endregion
    }
    /// <summary>
    /// 유저 데이터를 불러옵니다.
    /// </summary>
    public void ReadData()
    {
        Debug.Log("ReadData 메서드 호출, 데이터를 불러옵니다.");
        #region DEFAULT DATA
        Debug.Log("'USER' 테이블의 데이터를 조회하는 함수를 호출합니다.");
        var bro = Backend.GameData.GetMyData("USER", new Where());
        if (bro.IsSuccess())
        {
            Debug.Log("데이터 조회에 성공했습니다. : " + bro);

            // Json으로 리턴된 데이터를 받아옵니다.
            LitJson.JsonData gameDataJson = bro.FlattenRows();

            // 받아온 데이터의 갯수가 0이라면 데이터가 존재하지 않는 것입니다.
            if (gameDataJson.Count <= 0)
            {
                Debug.LogWarning("데이터가 존재하지 않습니다.");
            }
            else
            {
                Data data = new Data();

                data.Nick_Name = gameDataJson[0]["NICK_NAME"].ToString();
                data.ATK = double.Parse(gameDataJson[0]["ATK"].ToString());
                data.HP = double.Parse(gameDataJson[0]["HP"].ToString());
                data.Player_Money = double.Parse(gameDataJson[0]["PLAYER_MONEY"].ToString());
                data.DiaMond = int.Parse(gameDataJson[0]["DIAMOND"].ToString());
                data.Player_Level = int.Parse(gameDataJson[0]["PLAYER_LEVEL"].ToString());
                data.EXP = double.Parse(gameDataJson[0]["PLAYER_EXP"].ToString());
                data.Player_Stage = int.Parse(gameDataJson[0]["PLAYER_STAGE"].ToString());
                data.EXP_Upgrade_Count = int.Parse(gameDataJson[0]["EXP_UPGRADE_COUNT"].ToString());

                data.Buff_Timers[0] = float.Parse(gameDataJson[0]["BUFF_TIMER"][0].ToString());
                data.Buff_Timers[1] = float.Parse(gameDataJson[0]["BUFF_TIMER"][1].ToString());
                data.Buff_Timers[2] = float.Parse(gameDataJson[0]["BUFF_TIMER"][2].ToString());

                data.buff_x2_speed = float.Parse(gameDataJson[0]["SPEED"].ToString());
                data.Buff_Level = int.Parse(gameDataJson[0]["BUFF_LEVEL"].ToString());
                data.Buff_Level_Count = int.Parse(gameDataJson[0]["BUFF_LEVEL_COUNT"].ToString());
                data.Quest_Count = int.Parse(gameDataJson[0]["QUEST_COUNT"].ToString());


                data.Hero_Summon_Count = int.Parse(gameDataJson[0]["HERO_SUMMON_COUNT"].ToString());
                data.Hero_Pickup_Count = int.Parse(gameDataJson[0]["HERO_PICKUP_COUNT"].ToString());
                data.Relic_Summon_Count = int.Parse(gameDataJson[0]["RELIC_SUMMON_COUNT"].ToString());
                data.Relic_Pickup_Count = int.Parse(gameDataJson[0]["RELIC_PICKUP_COUNT"].ToString());

                data.StartDate = DateTime.Now.ToString();
                data.EndDate = gameDataJson[0]["END_DATE"].ToString();
                

                data.Daily_Enter_Key[0] = int.Parse(gameDataJson[0]["DAILY_ENTER_KEY"][0].ToString());
                data.Daily_Enter_Key[1] = int.Parse(gameDataJson[0]["DAILY_ENTER_KEY"][1].ToString());

                data.User_Key_Assets[0] = int.Parse(gameDataJson[0]["USER_KEY_ASSETS"][0].ToString());
                data.User_Key_Assets[1] = int.Parse(gameDataJson[0]["USER_KEY_ASSETS"][1].ToString());

                data.Dungeon_Clear_Level[0] = int.Parse(gameDataJson[0]["DUNGEON_CLEAR_LEVEL"][0].ToString());
                data.Dungeon_Clear_Level[1] = int.Parse(gameDataJson[0]["DUNGEON_CLEAR_LEVEL"][1].ToString());


                //if (string.IsNullOrEmpty(data.StartDate))
                //{
                //    /*data.EndDate가 null 또는 빈 문자열일때,
                //    DateTime.Parse가 호출되면 FormatException이 발생.
                //    메서드가 예외를 처리하지 않을 경우 메서드가 중도종료됨..*/

                //    Data_Manager.Main_Players_Data.StartDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //    Debug.Log("StartDate가 없어서 기본값으로 설정: " + data.StartDate);
                //}

                //if (string.IsNullOrEmpty(data.EndDate))
                //{
                //    /*data.EndDate가 null 또는 빈 문자열일때,
                //    DateTime.Parse가 호출되면 FormatException이 발생.
                //    메서드가 예외를 처리하지 않을 경우 메서드가 중도종료됨..*/

                //    Data_Manager.Main_Players_Data.EndDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //    Debug.Log("EndDate가 없어서 기본값으로 설정: " + data.EndDate);
                //}

                DateTime startDate = DateTime.Parse(data.StartDate);
                DateTime endDate = DateTime.Parse(data.EndDate);

                if (Get_Date_Dungeon_Item(startDate, endDate))
                {
                    data.Daily_Enter_Key[0] = 2;
                    data.Daily_Enter_Key[1] = 2;
                }

                Data_Manager.Main_Players_Data = data;

                Base_Manager.Data.Init();

                Debug.Log("USER 테이블 데이터를 정상적으로 불러와 데이터를 업데이트 하였습니다.");

            }
        }
        else
        {
            Debug.LogError("데이터 조회에 실패했습니다. : " + bro);
        }

        #endregion

        #region CHARACTER DATA
        Debug.Log("'CHARACTER' 테이블의 데이터를 조회하는 함수를 호출합니다.");
        var char_bro = Backend.GameData.GetMyData("CHARACTER", new Where());

        if (char_bro.Rows().Count > 0)
        {

            var rows = BackendReturnObject.Flatten(char_bro.Rows());

            foreach (JsonData row in rows)
            {
                if (row.ContainsKey("character"))
                {
                    string charJsonData = row["character"].ToString();
                    Dictionary<string, JsonData> characterData = JsonConvert.DeserializeObject<Dictionary<string, JsonData>>(charJsonData);


                    foreach (var dict in characterData.Keys)
                    {
                        int hero_level = characterData[dict].ContainsKey("Hero_Level")
                           ? int.Parse(characterData[dict]["Hero_Level"].ToString()) : 0;

                        // 'Hero_Card_Amount' 값 가져오기 (예외 방지)
                        int hero_card_amount = characterData[dict].ContainsKey("Hero_Card_Amount")
                            ? int.Parse(characterData[dict]["Hero_Card_Amount"].ToString()) : 0;

                        // Holder 객체 생성 후 Dictionary에 추가

                        Holder holder = new Holder { Hero_Level = hero_level, Hero_Card_Amount = hero_card_amount };

                        Base_Manager.Data.character_Holder[dict] = holder;
                    }
                }
                
            }

            Set_Character_Data_Dictionary();
            Base_Manager.Data.Init();

            Debug.Log("영웅 보유 데이터 불러오기에 성공하였습니다.");
        }
        else
        {
            Debug.LogError("영웅 보유 데이터 조회에 실패했습니다. : " + char_bro);
        }
        #endregion

        #region ITEM_DATA
        Debug.Log("'ITEM' 테이블의 데이터를 조회하는 함수를 호출합니다.");
        var item_bro = Backend.GameData.GetMyData("ITEM", new Where());

        if (item_bro.Rows().Count > 0)
        {

            var rows = BackendReturnObject.Flatten(item_bro.Rows());

            foreach (JsonData row in rows)
            {
                if (row.ContainsKey("Item"))
                {
                    string charJsonData = row["Item"].ToString();
                    Dictionary<string, JsonData> characterData = JsonConvert.DeserializeObject<Dictionary<string, JsonData>>(charJsonData);


                    foreach (var dict in characterData.Keys)
                    {
                        int Item_level = characterData[dict].ContainsKey("Hero_Level")
                           ? int.Parse(characterData[dict]["Hero_Level"].ToString()) : 0;

                        // 'Hero_Card_Amount' 값 가져오기 (예외 방지)
                        int Item_card_amount = characterData[dict].ContainsKey("Hero_Card_Amount")
                            ? int.Parse(characterData[dict]["Hero_Card_Amount"].ToString()) : 0;

                        // Holder 객체 생성 후 Dictionary에 추가

                        Holder holder = new Holder { Hero_Level = Item_level, Hero_Card_Amount = Item_card_amount };

                        Base_Manager.Data.Item_Holder[dict] = holder;
                    }
                }

            }
        
            Base_Manager.Data.Init();

            Debug.Log("인벤토리 데이터 불러오기에 성공하였습니다.");
        }
        else
        {
            Debug.LogError("인벤토리 데이터 조회에 실패했습니다. : " + item_bro);
        }

        #endregion

        #region SMELT_DATA

        Debug.Log("'SMELT' 테이블의 데이터를 조회하는 함수를 호출합니다.");
        var smelt_bro = Backend.GameData.GetMyData("SMELT", new Where());

        if (smelt_bro.Rows().Count > 0)
        {

            var rows = BackendReturnObject.Flatten(smelt_bro.Rows());

            foreach (JsonData row in rows)
            {
                if (row.ContainsKey("Smelt"))
                {
                    string Smelt_Json_Data = row["Smelt"].ToString();
                    List<Smelt_Holder> smeltList = JsonConvert.DeserializeObject<List<Smelt_Holder>>(Smelt_Json_Data);
                    Base_Manager.Data.User_Main_Data_Smelt_Array = smeltList;                 
                }

                else
                {
                    Debug.LogWarning("'Smelt' 데이터가 올바른 JSON 형식이 아닙니다.");
                }

            }

            Base_Manager.Data.Init();

            Debug.Log("영웅 각인 데이터 불러오기에 성공하였습니다.");
        }
        else
        {
            Debug.LogError("영웅 각인 데이터 조회에 실패했습니다. : " + smelt_bro);
        }
        #endregion
    }
    /// <summary>
    /// 날짜가 자정이 지났는지 확인하고, 데일리 입장권을 지급할 지 판단합니다.
    /// </summary>
    /// <param name="startdate"></param>
    /// <param name="enddate"></param>
    /// <returns></returns>
    private bool Get_Date_Dungeon_Item(DateTime startdate, DateTime enddate)
    {
        if (startdate.Day != enddate.Day)
        {
            return true;
        }

        else
        {
            return false;
        }
    }

    /// <summary>
    /// 로드된 Character_Holder를 이용하여, Data_Character_Dictionary에 매핑합니다.
    /// </summary>
    private void Set_Character_Data_Dictionary()
    {
        // Scriptable 파일과 매칭하여 Data_Character_Dictionary에 저장
        Base_Manager.Data.Data_Character_Dictionary.Clear(); // 기존 데이터를 초기화

        var scriptableCharacters = Resources.LoadAll<Character_Scriptable>("Scriptable/Character");

        foreach (var characterScriptable in scriptableCharacters)
        {
            if (Base_Manager.Data.character_Holder.ContainsKey(characterScriptable.M_Character_Name))
            {
                // 캐릭터 데이터를 Scriptable과 매칭
                var holderData = Base_Manager.Data.character_Holder[characterScriptable.M_Character_Name];

                var characterHolder = new Character_Holder
                {
                    Data = characterScriptable,
                    holder = holderData
                };

                Base_Manager.Data.Data_Character_Dictionary[characterScriptable.M_Character_Name] = characterHolder;

            }

        }
    }

}
