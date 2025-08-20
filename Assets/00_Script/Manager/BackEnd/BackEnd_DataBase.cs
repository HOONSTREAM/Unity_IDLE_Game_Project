using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System;
using BackEnd;
using BackEnd.BackndNewtonsoft.Json.Linq;
using LitJson;
using System.Threading.Tasks;
using UnityEngine.UIElements;

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
       
        DateTime now = Utils.Get_Server_Time();
        
        Data_Manager.Main_Players_Data.EndDate = now;

        string now_date_string = now.ToString("yyyy-MM-dd");

        // 여기서 자정 비교 후 일일 초기화
        if (Get_Date_Dungeon_Item(Data_Manager.Main_Players_Data.Last_Daily_Reset_Time, now_date_string))
        {
            Base_Canvas.instance.Get_MainGame_Error_UI().Initialize("일일 컨텐츠가 초기화 되었습니다.");
            Get_Daily_Contents_Reset();
            Data_Manager.Main_Players_Data.StartDate = now;
            Data_Manager.Main_Players_Data.Last_Daily_Reset_Time = now_date_string;
        }

        try
        {
            
            bool isMidnightRange = now.Hour == 0;

            await SaveDefaultData(isMidnightRange);
            await SaveCharacterData();
            await SaveItemData();
            await Save_Status_Item_Data();
            await SaveSmeltData();
            await SaveSetHeroData();
            await Save_Research_Stat_Data();
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
        param.Add("PLAYER_HIGH_STAGE", data.Player_Max_Stage);
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
        param.Add("isBUY_START_Package", data.isBuySTARTPackage);
        param.Add("isBUY_START_DIA", data.isBuy_START_DIA_PACK);
        param.Add("isBUY_DIAMOND_Package", data.isBuyDIAMONDPackage);
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
        param.Add("USER_DPS", data.USER_DPS);
        param.Add("USER_DPS_LEVEL", data.USER_DPS_LEVEL);
        param.Add("LAST_DAILY_RESET", data.Last_Daily_Reset_Time);
        param.Add("USER_DPS_REWARD", data.DPS_REWARD);
        param.Add("USER_STAGE_REWARD", data.STAGE_REWARD);

        param.Add("ADS_FREE_DIA", data.ADS_FREE_DIA);
        param.Add("ADS_FREE_STEEL", data.ADS_FREE_STEEL);
        param.Add("FREE_DIA", data.ADS_FREE_DIA);
        param.Add("FREE_COMB_SCROLL", data.FREE_COMB_SCROLL);
        param.Add("DIA_GACHA_COUNT", data.DIA_GACHA_COUNT);

        param.Add("Attendance_Day", data.Attendance_Day);
        param.Add("Get_Attendance_Reward", data.Get_Attendance_Reward);
        param.Add("Attendance_Date", data.Attendance_Last_Date);

        param.Add("is_BUY_DIA_PASS", data.isBUY_DIA_PASS);
        param.Add("DIA_ATTENDANCE_DAY", data.DIA_PASS_ATTENDANCE_DAY);
        param.Add("Get_DIA_PASS_REWARD", data.Get_DIA_PASS_Reward);
        param.Add("DIA_PASS_ATTENDANCE_DATE", data.DIA_PASS_Last_Date);


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
                    Backend.Leaderboard.User.UpdateMyDataAndRefreshLeaderboard(Utils.DPS_LEADERBOARD_UUID, "USER", inDate, param);
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
    private async Task Save_Status_Item_Data()
    {
        Param param = new Param();
        string json = JsonConvert.SerializeObject(Base_Manager.Data.Status_Item_Holder);
        param.Add("status_Item", json);

        Backend.GameData.Update("STATUS_ITEM", new Where(), param, callback =>
        {
            Debug.Log(callback.IsSuccess() ? "성장장비 저장 성공" : "성장장비 저장 실패");
        });

        await Task.Yield();
    }
    private async Task Save_Research_Stat_Data()
    {
        Param param = new Param();
        string json = JsonConvert.SerializeObject(Base_Manager.Data.User_Main_Data_Research_Array);
        param.Add("Research_Stat", json);

        Backend.GameData.Update("RESEARCH_STAT", new Where(), param, callback =>
        {
            Debug.Log(callback.IsSuccess() ? "연구정수 저장 성공" : "연구정수 저장 실패");
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
                if (!gameDataJson[0].ContainsKey("isBUY_START_Package"))
                {
                    Debug.Log("isBUY_START_Package 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("isBUY_START_Package", data.isBuySTARTPackage);

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
                    Debug.Log("isBUY_START_Package 컬럼이 존재합니다.");
                    data.isBuySTARTPackage = bool.Parse(gameDataJson[0]["isBUY_START_Package"].ToString());
                }
                if (!gameDataJson[0].ContainsKey("isBUY_START_DIA"))
                {
                    Debug.Log("isBUY_START_DIA 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("isBUY_START_DIA", data.isBuy_START_DIA_PACK);

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
                    Debug.Log("isBUY_START_DIA 컬럼이 존재합니다.");
                    data.isBuy_START_DIA_PACK = bool.Parse(gameDataJson[0]["isBUY_START_DIA"].ToString());
                }
                if (!gameDataJson[0].ContainsKey("isBUY_DIAMOND_Package"))
                {
                    Debug.Log("isBUY_DIAMOND_Package 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("isBUY_DIAMOND_Package", data.isBuyDIAMONDPackage);

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
                    Debug.Log("isBUY_DIAMOND_Package 컬럼이 존재합니다.");
                    data.isBuyDIAMONDPackage = bool.Parse(gameDataJson[0]["isBUY_DIAMOND_Package"].ToString());
                }
                if (!gameDataJson[0].ContainsKey("LAST_DAILY_RESET"))
                {
                    Debug.Log("LAST_DAILY_RESET 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("LAST_DAILY_RESET", data.Last_Daily_Reset_Time);

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
                    Debug.Log("LAST_DAILY_RESET 컬럼이 존재합니다.");
                    data.Last_Daily_Reset_Time = (gameDataJson[0]["LAST_DAILY_RESET"].ToString());
                }
                if (!gameDataJson[0].ContainsKey("USER_DPS"))
                {
                    Debug.Log("USER_DPS 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("USER_DPS", data.USER_DPS);

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
                    data.USER_DPS = double.Parse(gameDataJson[0]["USER_DPS"].ToString());
                }
                if (!gameDataJson[0].ContainsKey("USER_DPS_LEVEL"))
                {
                    Debug.Log("USER_DPS_LEVEL 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("USER_DPS_LEVEL", data.USER_DPS_LEVEL);

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
                    Debug.Log("USER_DPS_LEVEL 컬럼이 존재합니다.");
                    data.USER_DPS_LEVEL = int.Parse(gameDataJson[0]["USER_DPS_LEVEL"].ToString());
                }
                if (!gameDataJson[0].ContainsKey("USER_DPS_REWARD"))
                {
                    Debug.Log("USER_DPS_REWARD 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("USER_DPS_REWARD", data.DPS_REWARD);

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
                    Debug.Log("USER_DPS_REWARD 컬럼이 존재합니다.");
                    data.DPS_REWARD = (gameDataJson[0]["USER_DPS_REWARD"].ToString());
                }
                if (!gameDataJson[0].ContainsKey("USER_STAGE_REWARD"))
                {
                    Debug.Log("USER_STAGE_REWARD 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("USER_STAGE_REWARD", data.STAGE_REWARD);

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
                    Debug.Log("USER_STAGE_REWARD 컬럼이 존재합니다.");
                    data.STAGE_REWARD = (gameDataJson[0]["USER_STAGE_REWARD"].ToString());
                }
                if (!gameDataJson[0].ContainsKey("ADS_FREE_DIA"))
                {
                    Debug.Log("ADS_FREE_DIA 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("ADS_FREE_DIA", data.ADS_FREE_DIA);

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
                    Debug.Log("ADS_FREE_DIA 컬럼이 존재합니다.");
                    data.ADS_FREE_DIA = bool.Parse(gameDataJson[0]["ADS_FREE_DIA"].ToString());
                }
                if (!gameDataJson[0].ContainsKey("ADS_FREE_STEEL"))
                {
                    Debug.Log("ADS_FREE_STEEL 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("ADS_FREE_STEEL", data.ADS_FREE_STEEL);

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
                    Debug.Log("ADS_FREE_STEEL 컬럼이 존재합니다.");
                    data.ADS_FREE_STEEL = bool.Parse(gameDataJson[0]["ADS_FREE_STEEL"].ToString());
                }
                if (!gameDataJson[0].ContainsKey("FREE_DIA"))
                {
                    Debug.Log("FREE_DIA 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("FREE_DIA", data.FREE_DIA);

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
                    Debug.Log("FREE_DIA 컬럼이 존재합니다.");
                    data.FREE_DIA = bool.Parse(gameDataJson[0]["FREE_DIA"].ToString());
                }
                if (!gameDataJson[0].ContainsKey("FREE_COMB_SCROLL"))
                {
                    Debug.Log("FREE_COMB_SCROLL 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("FREE_COMB_SCROLL", data.FREE_COMB_SCROLL);

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
                    Debug.Log("FREE_COMB_SCROLL 컬럼이 존재합니다.");
                    data.FREE_COMB_SCROLL = bool.Parse(gameDataJson[0]["FREE_COMB_SCROLL"].ToString());
                }
                if (!gameDataJson[0].ContainsKey("DIA_GACHA_COUNT"))
                {
                    Debug.Log("DIA_GACHA_COUNT 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("DIA_GACHA_COUNT", data.DIA_GACHA_COUNT);

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
                    Debug.Log("DIA_GACHA_COUNT 컬럼이 존재합니다.");
                    data.DIA_GACHA_COUNT = int.Parse(gameDataJson[0]["DIA_GACHA_COUNT"].ToString());
                }
                if (!gameDataJson[0].ContainsKey("Attendance_Day"))
                {
                    Debug.Log("Attendance_Day 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("Attendance_Day", data.Attendance_Day);

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
                    Debug.Log("Attendance_Day 컬럼이 존재합니다.");
                    data.Attendance_Day = int.Parse(gameDataJson[0]["Attendance_Day"].ToString());
                }
                if (!gameDataJson[0].ContainsKey("Get_Attendance_Reward"))
                {
                    Debug.Log("Get_Attendance_Reward 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("Get_Attendance_Reward", data.Get_Attendance_Reward);

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
                    Debug.Log("Get_Attendance_Reward 컬럼이 존재합니다.");
                    data.Get_Attendance_Reward = bool.Parse(gameDataJson[0]["Get_Attendance_Reward"].ToString());
                }
                if (!gameDataJson[0].ContainsKey("Attendance_Date"))
                {
                    Debug.Log("Attendance_Date 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("Attendance_Date", data.Attendance_Last_Date);

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
                    Debug.Log("Attendance_Date 컬럼이 존재합니다.");
                    data.Attendance_Last_Date = (gameDataJson[0]["Attendance_Date"].ToString());
                }
                if (!gameDataJson[0].ContainsKey("PLAYER_HIGH_STAGE"))
                {
                    Debug.Log("PLAYER_HIGH_STAGE 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("PLAYER_HIGH_STAGE", data.Player_Max_Stage);

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
                    Debug.Log("PLAYER_HIGH_STAGE 컬럼이 존재합니다.");
                    data.Player_Max_Stage = int.Parse(gameDataJson[0]["PLAYER_HIGH_STAGE"].ToString());
                }
                if (gameDataJson[0]["USER_KEY_ASSETS"].Count == 2)
                {
                    Debug.Log("기존 유저의 User_Key_Assets 배열이 2개이므로, 3개로 확장합니다.");

                    // 새로운 배열 선언 및 기존 값 복사
                    
                    int temp_1 = int.Parse(gameDataJson[0]["USER_KEY_ASSETS"][0].ToString());
                    int temp_2 = int.Parse(gameDataJson[0]["USER_KEY_ASSETS"][1].ToString());


                    data.User_Key_Assets = new int[3] { temp_1, temp_2, 0 };
                }
                else
                {                  
                    data.User_Key_Assets[0] = int.Parse(gameDataJson[0]["USER_KEY_ASSETS"][0].ToString());
                    data.User_Key_Assets[1] = int.Parse(gameDataJson[0]["USER_KEY_ASSETS"][1].ToString());
                    data.User_Key_Assets[2] = int.Parse(gameDataJson[0]["USER_KEY_ASSETS"][2].ToString());
                }
                if (gameDataJson[0]["DAILY_ENTER_KEY"].Count == 2)
                {
                    Debug.Log("기존 유저의 Daily_Enter_Key 배열이 2개이므로, 3개로 확장합니다.");

                    // 새로운 배열 선언 및 기존 값 복사
                    int temp_1 = int.Parse(gameDataJson[0]["DAILY_ENTER_KEY"][0].ToString());
                    int temp_2 = int.Parse(gameDataJson[0]["DAILY_ENTER_KEY"][1].ToString());
                    data.Daily_Enter_Key = new int[3] { temp_1, temp_2, 3 }; // 기본값

                }
                else
                {                   
                    data.Daily_Enter_Key[0] = int.Parse(gameDataJson[0]["DAILY_ENTER_KEY"][0].ToString());
                    data.Daily_Enter_Key[1] = int.Parse(gameDataJson[0]["DAILY_ENTER_KEY"][1].ToString());
                    data.Daily_Enter_Key[2] = int.Parse(gameDataJson[0]["DAILY_ENTER_KEY"][2].ToString());
                }
                if (gameDataJson[0]["DUNGEON_CLEAR_LEVEL"].Count == 2)
                {
                    Debug.Log("기존 유저의 Dungeon_Clear_Level 배열이 2개이므로, 3개로 확장합니다.");

                    // 새로운 배열 선언 및 기존 값 복사
                   
                    int temp_1 = int.Parse(gameDataJson[0]["DUNGEON_CLEAR_LEVEL"][0].ToString());
                    int temp_2 = int.Parse(gameDataJson[0]["DUNGEON_CLEAR_LEVEL"][1].ToString());

                    data.Dungeon_Clear_Level = new int[3] { temp_1, temp_2, 0 }; // 기본값

                }
                else
                {
                    Debug.Log("DUNGEON_CLEAR_LEVEL 배열이 3개입니다.");
                    data.Dungeon_Clear_Level[0] = int.Parse(gameDataJson[0]["DUNGEON_CLEAR_LEVEL"][0].ToString());
                    data.Dungeon_Clear_Level[1] = int.Parse(gameDataJson[0]["DUNGEON_CLEAR_LEVEL"][1].ToString());
                    data.Dungeon_Clear_Level[2] = int.Parse(gameDataJson[0]["DUNGEON_CLEAR_LEVEL"][2].ToString());
                }
                if (!gameDataJson[0].ContainsKey("is_BUY_DIA_PASS"))
                {
                    Debug.Log("is_BUY_DIA_PASS 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("is_BUY_DIA_PASS", data.isBUY_DIA_PASS);

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
                    Debug.Log("is_BUY_DIA_PASS 컬럼이 존재합니다.");
                    data.isBUY_DIA_PASS = bool.Parse(gameDataJson[0]["is_BUY_DIA_PASS"].ToString());
                }
                if (!gameDataJson[0].ContainsKey("DIA_ATTENDANCE_DAY"))
                {
                    Debug.Log("DIA_ATTENDANCE_DAY 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("DIA_ATTENDANCE_DAY", data.DIA_PASS_ATTENDANCE_DAY);

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
                    Debug.Log("DIA_ATTENDANCE_DAY 컬럼이 존재합니다.");
                    data.DIA_PASS_ATTENDANCE_DAY = int.Parse(gameDataJson[0]["DIA_ATTENDANCE_DAY"].ToString());
                }
                if (!gameDataJson[0].ContainsKey("Get_DIA_PASS_REWARD"))
                {
                    Debug.Log("Get_DIA_PASS_REWARD 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("Get_DIA_PASS_REWARD", data.Get_DIA_PASS_Reward);

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
                    Debug.Log("Get_DIA_PASS_REWARD 컬럼이 존재합니다.");
                    data.Get_DIA_PASS_Reward = bool.Parse(gameDataJson[0]["Get_DIA_PASS_REWARD"].ToString());
                }
                if (!gameDataJson[0].ContainsKey("DIA_PASS_ATTENDANCE_DATE"))
                {
                    Debug.Log("DIA_PASS_ATTENDANCE_DATE 컬럼이 없어 새로 추가합니다.");
                    Param param = new Param();
                    param.Add("DIA_PASS_ATTENDANCE_DATE", data.DIA_PASS_Last_Date);

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
                    Debug.Log("DIA_PASS_ATTENDANCE_DATE 컬럼이 존재합니다.");
                    data.DIA_PASS_Last_Date = (gameDataJson[0]["DIA_PASS_ATTENDANCE_DATE"].ToString());
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


                DateTime serverNow = Utils.Get_Server_Time();
                string today = serverNow.ToString("yyyy-MM-dd");
                
                if(data.Last_Daily_Reset_Time != today)
                {
                    Get_Daily_Contents_Reset();

                    Param param = new Param();
                    param.Add("LAST_DAILY_RESET", today);

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

                Data_Manager.Main_Players_Data = data;
                Data_Manager.Main_Players_Data.Player_Tier = (Player_Tier)tier_number;

                #region 시즌제 적용 티어 초기화
                if (Utils.TIER_SEASON > data.Season)
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

        #region STATUS_ITEM_DATA
        Debug.Log("'STATUS_ITEM' 테이블의 데이터를 조회하는 함수를 호출합니다.");
        var status_item_bro = Backend.GameData.GetMyData("STATUS_ITEM", new Where());

        // 1. 응답 자체가 실패한 경우 - Insert하지 않음
        if (!status_item_bro.IsSuccess())
        {
            Debug.LogError("성장장비 데이터 조회 실패 - 서버 오류 : " + status_item_bro);          
        }

        // 2. 조회는 성공했으나, 유저 데이터가 존재하지 않는 경우 - Insert
        if (status_item_bro.Rows().Count == 0)
        {
            Debug.LogWarning("성장장비 데이터 없음. 새로 Insert 시도.");

            Param status_item_param = new Param();
            status_item_param.Add("status_Item", Base_Manager.Data.Status_Item_Holder);

            var insertResult = Backend.GameData.Insert("STATUS_ITEM", status_item_param);
            if (insertResult.IsSuccess())
            {
                Debug.Log("성장장비 데이터 추가 성공 : " + insertResult);
            }
            else
            {
                Debug.LogError("성장장비 데이터 추가 실패 : " + insertResult);
            }         
        }

        // 3. 데이터 존재할 경우 파싱 처리
        var status_rows = BackendReturnObject.Flatten(status_item_bro.Rows());

        foreach (JsonData row in status_rows)
        {
            if (row.ContainsKey("status_Item"))
            {
                string charJsonData = row["status_Item"].ToString();
                Dictionary<string, JsonData> status_item_Data = JsonConvert.DeserializeObject<Dictionary<string, JsonData>>(charJsonData);

                foreach (var dict in status_item_Data.Keys)
                {
                    int Enhancement = status_item_Data[dict].ContainsKey("Enhancement") ? int.Parse(status_item_Data[dict]["Enhancement"].ToString()) : 0;                   
                    double Additional_ATK = status_item_Data[dict].ContainsKey("Additional_ATK") ? double.Parse(status_item_Data[dict]["Additional_ATK"].ToString()) : 0;
                    double Additional_HP = status_item_Data[dict].ContainsKey("Additional_HP") ? double.Parse(status_item_Data[dict]["Additional_HP"].ToString()) : 0;
                    double Additional_STR = status_item_Data[dict].ContainsKey("Additional_STR") ? double.Parse(status_item_Data[dict]["Additional_STR"].ToString()) : 0;
                    double Additional_DEX = status_item_Data[dict].ContainsKey("Additional_DEX") ? double.Parse(status_item_Data[dict]["Additional_DEX"].ToString()) : 0;
                    double Additional_VIT = status_item_Data[dict].ContainsKey("Additional_VIT") ? double.Parse(status_item_Data[dict]["Additional_VIT"].ToString()) : 0;
                    int Item_Level = status_item_Data[dict].ContainsKey("Item_Level") ? int.Parse(status_item_Data[dict]["Item_Level"].ToString()) : 0;
                    int Item_Amount = status_item_Data[dict].ContainsKey("Item_Amount") ? int.Parse(status_item_Data[dict]["Item_Amount"].ToString()) : 0;

                    Status_Item_Holder holder = new Status_Item_Holder
                    {
                        Enhancement = Enhancement,                       
                        Additional_ATK = Additional_ATK,
                        Additional_HP = Additional_HP,
                        Additional_STR = Additional_STR,
                        Additional_DEX = Additional_DEX,
                        Additional_VIT = Additional_VIT,
                        Item_Level = Item_Level,
                        Item_Amount = Item_Amount
                    };

                    Base_Manager.Data.Status_Item_Holder[dict] = holder;
                }
            }
        }

        Base_Manager.Data.Init();
        Debug.Log("성장장비 데이터 불러오기에 성공하였습니다.");
        #endregion

        #region RESEARCH_STAT_DATA
        Debug.Log("'RESEARCH_STAT' 테이블의 데이터를 조회하는 함수를 호출합니다.");
        var Research_bro = Backend.GameData.GetMyData("RESEARCH_STAT", new Where());

        // 1. 응답 자체가 실패한 경우 - Insert하지 않음
        if (!Research_bro.IsSuccess())
        {
            Debug.LogError("연구정수 데이터 조회 실패 - 서버 오류 : " + Research_bro);
        }

        // 2. 조회는 성공했으나, 유저 데이터가 존재하지 않는 경우 - Insert
        if (Research_bro.Rows().Count == 0)
        {
            Debug.LogWarning("연구정수 데이터 없음. 새로 Insert 시도.");

            Param research_param = new Param();
            research_param.Add("Research_Stat", Base_Manager.Data.User_Main_Data_Research_Array);

            var insertResult = Backend.GameData.Insert("RESEARCH_STAT", research_param);
            if (insertResult.IsSuccess())
            {
                Debug.Log("연구정수 데이터 추가 성공 : " + insertResult);
            }
            else
            {
                Debug.LogError("연구정수 데이터 추가 실패 : " + insertResult);
            }
        }

        // 3. 데이터 존재할 경우 파싱 처리
        if (Research_bro.Rows().Count > 0)
        {

            var rows = BackendReturnObject.Flatten(Research_bro.Rows());

            foreach (JsonData row in rows)
            {
                if (row.ContainsKey("Research_Stat"))
                {
                    string Research_Json_Data = row["Research_Stat"].ToString();
                    List<Research_Holder> research_List = JsonConvert.DeserializeObject<List<Research_Holder>>(Research_Json_Data);
                    Base_Manager.Data.User_Main_Data_Research_Array = research_List;
                }

                else
                {
                    Debug.LogWarning("'연구정수' 데이터가 올바른 JSON 형식이 아닙니다.");
                }

            }

            Base_Manager.Data.Init();

            Debug.Log("영웅정수 데이터 불러오기에 성공하였습니다.");
        }
        else
        {
            Debug.LogError("영웅정수 데이터 조회에 실패했습니다. : " + Research_bro);
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
    /// 데일리 컨텐츠 초기화를 진행합니다.
    /// </summary>
    public void Get_Daily_Contents_Reset()
    {
        var data = Data_Manager.Main_Players_Data;

        //던전 일일 입장권 초기화
        data.Daily_Enter_Key[0] = 2;
        data.Daily_Enter_Key[1] = 2;
        data.Daily_Enter_Key[2] = 2;

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
        data.isBuyDIAMONDPackage = false;

        data.ADS_FREE_DIA = false;
        data.ADS_FREE_STEEL = false;
        data.FREE_DIA = false;
        data.FREE_COMB_SCROLL = false;
        data.DIA_GACHA_COUNT = 0;

        data.Get_Attendance_Reward = false;
        data.Get_DIA_PASS_Reward = false;
    }

    /// <summary>
    /// 날짜가 자정이 지났는지 확인하고, 데일리 입장권을 지급할 지 판단합니다.
    /// </summary>
    /// <param name="startdate"></param>
    /// <param name="enddate"></param>
    /// <returns></returns>
    public bool Get_Date_Dungeon_Item(string startdate, string enddate)
    {
        if (startdate != enddate)
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
