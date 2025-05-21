using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using BackEnd;
using BackEnd.BackndNewtonsoft.Json.Linq;
using LitJson;
using System.Threading.Tasks;

public partial class BackEnd_Manager : MonoBehaviour
{
    /// <summary>
    /// 유저 데이터를 저장합니다. 00시~01시 사이는 서버시간 체크하여, 리더보드 순위 등록을 생략합니다.
    /// </summary>
    /// <returns></returns>
    public async Task WriteData()
    {
        Debug.Log("WriteData 메서드 호출, 데이터를 기록합니다.");

        if (Data_Manager.Main_Players_Data == null)
        {
            Debug.LogError("데이터가 존재하지 않습니다. Initialize 통해 데이터를 생성해주세요.");
            return;
        }

        try
        {
            DateTime now = Utils.Get_Server_Time();
            bool isMidnightRange = now.Hour == 0;

            await SaveDefaultData(isMidnightRange);
            await SaveCharacterData();
            await SaveItemData();
            await SaveSmeltData();
            await SaveSetHeroData();
            await SaveSetRelicData();
        }
        catch (Exception e)
        {
            Debug.LogError($"[ERROR] WriteData 실행 중 예외 발생: {e.Message}");
        }
    } 
    private async Task SaveDefaultData(bool skipLeaderboard)
    {
        Param param = new Param();

        var data = Data_Manager.Main_Players_Data;
        param.Add("ATK", data.ATK);
        param.Add("HP", data.HP);
        param.Add("PLAYER_TIER", (int)data.Player_Tier);
        param.Add("PLAYER_MONEY", data.Player_Money);
        param.Add("DIAMOND", data.DiaMond);
        param.Add("PLAYER_LEVEL", data.Player_Level);
        param.Add("PLAYER_EXP", data.EXP);
        param.Add("PLAYER_STAGE", data.Player_Stage);
        param.Add("EXP_UPGRADE_COUNT", data.EXP_Upgrade_Count);
        param.Add("BUFF_TIMER", data.Buff_Timers);
        param.Add("ADS_TIMER", data.ADS_Timer);
        param.Add("SPEED", data.buff_x2_speed);
        param.Add("QUEST_COUNT", data.Quest_Count);
        param.Add("HERO_SUMMON_COUNT", data.Hero_Summon_Count);
        param.Add("HERO_PICKUP_COUNT", data.Hero_Pickup_Count);
        param.Add("RELIC_SUMMON_COUNT", data.Relic_Summon_Count);
        param.Add("RELIC_PICKUP_COUNT", data.Relic_Pickup_Count);
        param.Add("START_DATE", data.StartDate);
        param.Add("END_DATE", Utils.Get_Server_Time());
        param.Add("DAILY_ENTER_KEY", data.Daily_Enter_Key);
        param.Add("USER_KEY_ASSETS", data.User_Key_Assets);
        param.Add("DUNGEON_CLEAR_LEVEL", data.Dungeon_Clear_Level);
        param.Add("isBUY_AD_Package", data.isBuyADPackage);
        param.Add("isBUY_LAUNCH_EVENT", data.isBuyLAUNCH_EVENT);
        param.Add("isBUY_TODAY_Package", data.isBuyTodayPackage);
        param.Add("isBUY_STRONG_Package", data.isBuySTRONGPackage);
        param.Add("ADS_HERO_SUMMON_COUNT", data.ADS_Hero_Summon_Count);
        param.Add("ADS_RELIC_SUMMON_COUNT", data.ADS_Relic_Summon_Count);

        data.Event_Push_Alarm_Agree = Utils.is_push_alarm_agree;

        param.Add("EVENT_PUSH_ALARM", data.Event_Push_Alarm_Agree);

        param.Add("Daily_Attendance", data.Daily_Attendance);
        param.Add("Daily_Levelup", data.Levelup);
        param.Add("Daily_Summon", data.Summon);
        param.Add("Daily_Relic", data.Relic);
        param.Add("Daily_Dungeon_Gold", data.Dungeon_Gold);
        param.Add("Daily_Dungeon_Dia", data.Dungeon_Dia);
        param.Add("Daily_Attendance_Clear", data.Daily_Attendance_Clear);
        param.Add("Daily_Levelup_Clear", data.Level_up_Clear);
        param.Add("Daily_Summon_Clear", data.Summon_Clear);
        param.Add("Daily_Relic_Clear", data.Relic_Clear);
        param.Add("Daily_Dungeon_Gold_Clear", data.Dungeon_Gold_Clear);
        param.Add("Daily_Dungeon_Dia_Clear", data.Dungeon_Dia_Clear);
        param.Add("Fast_Mode", data.isFastMode);

        data.Season = Utils.TIER_SEASON;

        param.Add("SEASON", data.Season);
        
        var bro = Backend.GameData.Get("USER", new Where());
        
        if (!bro.IsSuccess()) return;

        string inDate = bro.GetInDate();

        
        if (skipLeaderboard)
        {
            Backend.GameData.Update("USER", new Where(), param, callback =>
            {
                Debug.Log(callback.IsSuccess() ? "데이터 저장 (리더보드 제외) 성공" : "데이터 저장 실패");
            });
        }
        else
        {
            Backend.Leaderboard.User.UpdateMyDataAndRefreshLeaderboard(Utils.LEADERBOARD_UUID, "USER", inDate, param, callback =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("리더보드와 데이터 저장 성공");
                }
                else
                {
                    Debug.LogWarning("리더보드 갱신 실패, 등록 여부 확인 후 처리");
                    TryReRegisterLeaderboard(param);
                }
            });
        }

        await Task.Yield();
    }
    private void TryReRegisterLeaderboard(Param param)
    {
        Backend.Leaderboard.User.GetMyLeaderboard(Utils.LEADERBOARD_UUID, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.Log("리더보드 미등록 상태, 등록 시도");
                var bro = Backend.GameData.GetMyData("USER", new Where());
                string inDate = bro.GetInDate();
                var reg = Backend.URank.User.UpdateUserScore(Utils.LEADERBOARD_UUID, "USER", inDate, param);

                if (reg.IsSuccess())
                {
                    Debug.Log("리더보드 등록 성공");
                }
                else
                {
                    Debug.LogError("리더보드 등록 실패, 일반 데이터 저장 진행");
                    Backend.GameData.Update("USER", new Where(), param, fallback =>
                    {
                        Debug.Log($"일반 저장 결과: {fallback}");
                    });
                }
            }
        });
    }
    private async Task SaveCharacterData()
    {
        Param character_param = new Param();
        string json = JsonConvert.SerializeObject(Base_Manager.Data.character_Holder);
        character_param.Add("character", json);

        Backend.GameData.Update("CHARACTER", new Where(), character_param, callback =>
        {
            Debug.Log(callback.IsSuccess() ? "영웅 데이터 저장 성공" : "영웅 데이터 저장 실패");
        });

        await Task.Yield();
    }
    private async Task SaveItemData()
    {
        Param param = new Param();
        string json = JsonConvert.SerializeObject(Base_Manager.Data.Item_Holder);
        param.Add("Item", json);

        Backend.GameData.Update("ITEM", new Where(), param, callback =>
        {
            Debug.Log(callback.IsSuccess() ? "아이템 저장 성공" : "아이템 저장 실패");
        });

        await Task.Yield();
    }
    private async Task SaveSmeltData()
    {
        Param param = new Param();
        string json = JsonConvert.SerializeObject(Base_Manager.Data.User_Main_Data_Smelt_Array);
        param.Add("Smelt", json);

        Backend.GameData.Update("SMELT", new Where(), param, callback =>
        {
            Debug.Log(callback.IsSuccess() ? "각인 저장 성공" : "각인 저장 실패");
        });

        await Task.Yield();
    }
    private async Task SaveSetHeroData()
    {
        Param param = new Param();
        string json = JsonConvert.SerializeObject(Base_Manager.Character.Set_Character);
        param.Add("Player_Set_Hero", json);

        Backend.GameData.Update("PLAYER_SET_HERO", new Where(), param, callback =>
        {
            Debug.Log(callback.IsSuccess() ? "영웅 배치 저장 성공" : "영웅 배치 저장 실패");
        });

        await Task.Yield();
    }
    private async Task SaveSetRelicData()
    {
        Param param = new Param();
        string json = JsonConvert.SerializeObject(Base_Manager.Data.Main_Set_Item);
        param.Add("Player_Set_Relic", json);

        Backend.GameData.Update("PLAYER_SET_RELIC", new Where(), param, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("유물 배치 저장 성공");

                if (Relic_Manager.instance != null)
                {
                    Relic_Manager.instance.Initalize();
                }
                else
                {
                    Debug.LogWarning("Relic_Manager 인스턴스가 존재하지 않아 초기화 건너뜀");
                }
            }
            else
            {
                Debug.LogError("유물 배치 저장 실패");
            }
        });

        await Task.Yield();
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

                #region 신규 칼럼 데이터
               
                if (!gameDataJson[0].ContainsKey("SEASON"))
                {
                    Debug.Log("SEASON 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("SEASON", data.Season);

                    var bro_Get_Table_USER = Backend.GameData.Get("USER", new Where());

                    if (!bro_Get_Table_USER.IsSuccess()) return;

                    string inDate = bro_Get_Table_USER.GetInDate();

                    DateTime now = Utils.Get_Server_Time();
                    bool isMidnightRange = now.Hour == 0;

                    if (isMidnightRange)
                    {
                        Backend.GameData.Update("USER", new Where(), param, callback =>
                        {
                            Debug.Log(callback.IsSuccess() ? "데이터 저장 (리더보드 제외) 성공" : "데이터 저장 실패");
                        });
                    }
                    else
                    {
                        Backend.Leaderboard.User.UpdateMyDataAndRefreshLeaderboard(Utils.LEADERBOARD_UUID, "USER", inDate, param, callback =>
                        {
                            if (callback.IsSuccess())
                            {
                                Debug.Log("리더보드와 데이터 저장 성공");
                            }
                            else
                            {
                                Debug.LogWarning("리더보드 갱신 실패, 등록 여부 확인 후 처리");
                                TryReRegisterLeaderboard(param);
                            }
                        });
                    }                   
                }
                else
                {
                    Debug.Log("SEASON 컬럼이 존재합니다.");
                    data.Season = int.Parse(gameDataJson[0]["SEASON"].ToString());
                }

                #endregion

                data.ATK = double.Parse(gameDataJson[0]["ATK"].ToString());
                data.HP = double.Parse(gameDataJson[0]["HP"].ToString());
                int tier_number = int.Parse(gameDataJson[0]["PLAYER_TIER"].ToString());
                data.Player_Money = double.Parse(gameDataJson[0]["PLAYER_MONEY"].ToString());
                data.DiaMond = int.Parse(gameDataJson[0]["DIAMOND"].ToString());
                data.Player_Level = int.Parse(gameDataJson[0]["PLAYER_LEVEL"].ToString());
                data.EXP = double.Parse(gameDataJson[0]["PLAYER_EXP"].ToString());
                data.Player_Stage = int.Parse(gameDataJson[0]["PLAYER_STAGE"].ToString());
                data.EXP_Upgrade_Count = int.Parse(gameDataJson[0]["EXP_UPGRADE_COUNT"].ToString());

                data.Buff_Timers[0] = float.Parse(gameDataJson[0]["BUFF_TIMER"][0].ToString());
                data.Buff_Timers[1] = float.Parse(gameDataJson[0]["BUFF_TIMER"][1].ToString());
                data.Buff_Timers[2] = float.Parse(gameDataJson[0]["BUFF_TIMER"][2].ToString());

                data.ADS_Timer[0] = float.Parse(gameDataJson[0]["ADS_TIMER"][0].ToString());
                data.ADS_Timer[1] = float.Parse(gameDataJson[0]["ADS_TIMER"][1].ToString());

                data.buff_x2_speed = float.Parse(gameDataJson[0]["SPEED"].ToString());            
                data.Quest_Count = int.Parse(gameDataJson[0]["QUEST_COUNT"].ToString());


                data.Hero_Summon_Count = int.Parse(gameDataJson[0]["HERO_SUMMON_COUNT"].ToString());
                data.Hero_Pickup_Count = int.Parse(gameDataJson[0]["HERO_PICKUP_COUNT"].ToString());
                data.Relic_Summon_Count = int.Parse(gameDataJson[0]["RELIC_SUMMON_COUNT"].ToString());
                data.Relic_Pickup_Count = int.Parse(gameDataJson[0]["RELIC_PICKUP_COUNT"].ToString());


                data.EndDate = DateTime.Parse(gameDataJson[0]["END_DATE"].ToString());
                data.StartDate = Utils.Get_Server_Time();

                data.Daily_Enter_Key[0] = int.Parse(gameDataJson[0]["DAILY_ENTER_KEY"][0].ToString());
                data.Daily_Enter_Key[1] = int.Parse(gameDataJson[0]["DAILY_ENTER_KEY"][1].ToString());

                data.User_Key_Assets[0] = int.Parse(gameDataJson[0]["USER_KEY_ASSETS"][0].ToString());
                data.User_Key_Assets[1] = int.Parse(gameDataJson[0]["USER_KEY_ASSETS"][1].ToString());

                data.Dungeon_Clear_Level[0] = int.Parse(gameDataJson[0]["DUNGEON_CLEAR_LEVEL"][0].ToString());
                data.Dungeon_Clear_Level[1] = int.Parse(gameDataJson[0]["DUNGEON_CLEAR_LEVEL"][1].ToString());

                DateTime startDate = data.StartDate;
                DateTime endDate = data.EndDate;


                data.isBuyADPackage = bool.Parse(gameDataJson[0]["isBUY_AD_Package"].ToString());
                data.isBuyLAUNCH_EVENT = bool.Parse(gameDataJson[0]["isBUY_LAUNCH_EVENT"].ToString());
                data.isBuySTRONGPackage = bool.Parse(gameDataJson[0]["isBUY_STRONG_Package"].ToString());
                data.isBuyTodayPackage = bool.Parse(gameDataJson[0]["isBUY_TODAY_Package"].ToString());
                data.Event_Push_Alarm_Agree = bool.Parse(gameDataJson[0]["EVENT_PUSH_ALARM"].ToString());
                data.Daily_Attendance = int.Parse(gameDataJson[0]["Daily_Attendance"].ToString());
                data.Levelup = int.Parse(gameDataJson[0]["Daily_Levelup"].ToString());
                data.Summon = int.Parse(gameDataJson[0]["Daily_Summon"].ToString());
                data.Relic = int.Parse(gameDataJson[0]["Daily_Relic"].ToString());
                data.Dungeon_Gold = int.Parse(gameDataJson[0]["Daily_Dungeon_Gold"].ToString());
                data.Dungeon_Dia = int.Parse(gameDataJson[0]["Daily_Dungeon_Dia"].ToString());

                data.Daily_Attendance_Clear = bool.Parse(gameDataJson[0]["Daily_Attendance_Clear"].ToString());
                data.Level_up_Clear = bool.Parse(gameDataJson[0]["Daily_Levelup_Clear"].ToString());
                data.Summon_Clear = bool.Parse(gameDataJson[0]["Daily_Summon_Clear"].ToString());
                data.Relic_Clear = bool.Parse(gameDataJson[0]["Daily_Relic_Clear"].ToString());
                data.Dungeon_Gold_Clear = bool.Parse(gameDataJson[0]["Daily_Dungeon_Gold_Clear"].ToString());
                data.Dungeon_Dia_Clear = bool.Parse(gameDataJson[0]["Daily_Dungeon_Dia_Clear"].ToString());
                data.ADS_Hero_Summon_Count = int.Parse(gameDataJson[0]["ADS_HERO_SUMMON_COUNT"].ToString());
                data.ADS_Relic_Summon_Count = int.Parse(gameDataJson[0]["ADS_RELIC_SUMMON_COUNT"].ToString());
                data.isFastMode = bool.Parse(gameDataJson[0]["Fast_Mode"].ToString());
                


                if (Get_Date_Dungeon_Item(startDate, endDate))
                {
                    //던전 일일 입장권 초기화
                    data.Daily_Enter_Key[0] = 2;
                    data.Daily_Enter_Key[1] = 2;

                    //일일 퀘스트 초기화
                    data.Daily_Attendance = 1;
                    data.Levelup = 0;
                    data.Summon = 0;
                    data.Dungeon_Dia = 0;
                    data.Dungeon_Gold = 0;
                    data.Relic = 0;

                    data.Daily_Attendance_Clear = false;
                    data.Level_up_Clear = false;
                    data.Summon_Clear = false;
                    data.Dungeon_Dia_Clear = false;
                    data.Dungeon_Gold_Clear = false;
                    data.Relic_Clear = false;

                    data.ADS_Hero_Summon_Count = 0;
                    data.ADS_Relic_Summon_Count = 0;

                    data.isBuyTodayPackage = false;
                    data.isBuySTRONGPackage = false;

                }

                Data_Manager.Main_Players_Data = data;
                Data_Manager.Main_Players_Data.Player_Tier = (Player_Tier)tier_number;

                #region 시즌제 적용 티어 초기화
                if (Utils.TIER_SEASON >= data.Season)
                {
                    data.Player_Tier = Player_Tier.Tier_Beginner;
                    data.Season = Utils.TIER_SEASON;
                }
                #endregion

                Utils.Calculate_ADS_Timer(); // 오프라인 시간만큼 광고 락 시간 차감
                Utils.Calculate_ADS_Buff_Timer(); // 오프라인 시간만큼 광고버프 시간 차감
                Utils.Calculate_ADS_X2_SPEED_Timer(); // 오프라인 시간만큼 광고 2배속 시간 차감


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

        #region PLAYER_SET_HERO_DATA

        Debug.Log("'PLAYER_SET_HERO' 테이블의 데이터를 조회하는 함수를 호출합니다.");
        var player_set_hero_bro = Backend.GameData.GetMyData("PLAYER_SET_HERO", new Where());

        if (player_set_hero_bro.Rows().Count > 0)
        {

            var rows = BackendReturnObject.Flatten(player_set_hero_bro.Rows());

            foreach (JsonData row in rows)
            {
                if (row.ContainsKey("Player_Set_Hero"))
                {
                    string Player_Set_Hero_Data = row["Player_Set_Hero"].ToString();

                    if (string.IsNullOrEmpty(Player_Set_Hero_Data))
                    {
                        Debug.LogError("영웅 배치 데이터가 비어있습니다.");
                        return;
                    }
                    Character_Holder[] char_holder = JsonConvert.DeserializeObject<Character_Holder[]>(Player_Set_Hero_Data);
                    Base_Manager.Character.Set_Character = char_holder;
                }

                else
                {
                    Debug.LogWarning(" 'Player_Set_Hero' 데이터가 올바른 JSON 형식이 아닙니다.");
                }

            }


            Base_Manager.Data.Init();
            
            Debug.Log("영웅 배치 데이터 불러오기에 성공하였습니다.");
        }
        else
        {
            Debug.LogError("영웅 배치 데이터 조회에 실패했습니다. : " + player_set_hero_bro);
        }
        #endregion

        #region PLAYER_SET_RELIC_DATA

        Debug.Log("'PLAYER_SET_RELIC' 테이블의 데이터를 조회하는 함수를 호출합니다.");
        var player_set_relic_bro = Backend.GameData.GetMyData("PLAYER_SET_RELIC", new Where());

        if (player_set_relic_bro.Rows().Count > 0)
        {

            var rows = BackendReturnObject.Flatten(player_set_relic_bro.Rows());

            foreach (JsonData row in rows)
            {
                if (row.ContainsKey("Player_Set_Relic"))
                {
                    string Player_Set_Relic_Data = row["Player_Set_Relic"].ToString();

                    if (string.IsNullOrEmpty(Player_Set_Relic_Data))
                    {
                        Debug.LogError("유물 배치 데이터가 비어있습니다.");
                        return;
                    }
                    Item_Scriptable[] relic_holder = JsonConvert.DeserializeObject<Item_Scriptable[]>(Player_Set_Relic_Data);
                    Base_Manager.Data.Main_Set_Item = relic_holder;
                }

                else
                {
                    Debug.LogWarning(" 'Player_Set_Relic' 데이터가 올바른 JSON 형식이 아닙니다.");
                }

            }


            Base_Manager.Data.Init();

            Debug.Log("유물 배치 데이터 불러오기에 성공하였습니다.");
        }
        else
        {
            Debug.LogError("유물 배치 데이터 조회에 실패했습니다. : " + player_set_relic_bro);
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
            if (Base_Manager.Data.character_Holder.ContainsKey(characterScriptable.Character_EN_Name))
            {
                // 캐릭터 데이터를 Scriptable과 매칭
                var holderData = Base_Manager.Data.character_Holder[characterScriptable.Character_EN_Name];

                var characterHolder = new Character_Holder
                {
                    Data = characterScriptable,
                    holder = holderData
                };

                Base_Manager.Data.Data_Character_Dictionary[characterScriptable.Character_EN_Name] = characterHolder;

            }

        }
    }

}
