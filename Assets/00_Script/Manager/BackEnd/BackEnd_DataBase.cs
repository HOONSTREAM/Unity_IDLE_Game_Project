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
    /// 유저 데이터를 저장합니다. BackendReturnObject는 서버와 통신한 결과값을 의미합니다.
    /// </summary>
    public async Task WriteData()
    {
       
        Debug.Log("WriteData 메서드 호출, 데이터를 기록합니다.");

            #region DEFAULT DATA

        if (Data_Manager.Main_Players_Data == null)
        {
            Debug.LogError("데이터가 존재하지 않습니다. Initialize 통해 데이터를 생성해주세요.");
            return;
        }

        try
        {
            Param param = new Param();

            param.Add("ATK", Data_Manager.Main_Players_Data.ATK); // 플레이어 공격력
            param.Add("HP", Data_Manager.Main_Players_Data.HP); // 플레이어 체력
            param.Add("PLAYER_MONEY", Data_Manager.Main_Players_Data.Player_Money); // 플레이어 골드 소지량
            param.Add("DIAMOND", Data_Manager.Main_Players_Data.DiaMond); //플레이어 다이아몬드 소지량
            param.Add("PLAYER_LEVEL", Data_Manager.Main_Players_Data.Player_Level); // 플레이어 레벨
            param.Add("PLAYER_EXP", Data_Manager.Main_Players_Data.EXP); // 플레이어 경험치
            param.Add("PLAYER_STAGE", Data_Manager.Main_Players_Data.Player_Stage); // 플레이어 스테이지
            param.Add("EXP_UPGRADE_COUNT", Data_Manager.Main_Players_Data.EXP_Upgrade_Count); // 레벨업 버튼 카운트
            param.Add("BUFF_TIMER", Data_Manager.Main_Players_Data.Buff_Timers); // 광고버프
            param.Add("ADS_TIMER", Data_Manager.Main_Players_Data.ADS_Timer); // 소환 광고 시청 락타임
            param.Add("SPEED", Data_Manager.Main_Players_Data.buff_x2_speed); // 2배속       
            param.Add("QUEST_COUNT", Data_Manager.Main_Players_Data.Quest_Count); // 퀘스트 단계
            param.Add("HERO_SUMMON_COUNT", Data_Manager.Main_Players_Data.Hero_Summon_Count); // 영웅소환 카운트
            param.Add("HERO_PICKUP_COUNT", Data_Manager.Main_Players_Data.Hero_Pickup_Count); // 영웅 확정소환 카운트
            param.Add("RELIC_SUMMON_COUNT", Data_Manager.Main_Players_Data.Relic_Summon_Count); // 유물소환 횟수 카운트
            param.Add("RELIC_PICKUP_COUNT", Data_Manager.Main_Players_Data.Relic_Pickup_Count); // 확정소환 카운트
            param.Add("START_DATE", Data_Manager.Main_Players_Data.StartDate); // 게임 시작시간
            param.Add("END_DATE", Utils.Get_Server_Time()); // 서버시간        
            param.Add("DAILY_ENTER_KEY", Data_Manager.Main_Players_Data.Daily_Enter_Key); // 데일리 지급 던전 입장키
            param.Add("USER_KEY_ASSETS", Data_Manager.Main_Players_Data.User_Key_Assets); // 유저가 구매나, 추가로 지급받은 던전 입장키
            param.Add("DUNGEON_CLEAR_LEVEL", Data_Manager.Main_Players_Data.Dungeon_Clear_Level); // 최종 던전 클리어 레벨 (배열)


            param.Add("isBUY_AD_Package", Data_Manager.Main_Players_Data.isBuyADPackage); // 광고제거 패키지 구매여부
            param.Add("ADS_HERO_SUMMON_COUNT", Data_Manager.Main_Players_Data.ADS_Hero_Summon_Count); // 광고시청 영웅소환 카운트
            param.Add("ADS_RELIC_SUMMON_COUNT", Data_Manager.Main_Players_Data.ADS_Relic_Summon_Count); // 광고시청 유물소환 카운트
            param.Add("EVENT_PUSH_ALARM", Data_Manager.Main_Players_Data.Event_Push_Alarm_Agree); // 푸시알람 동의여부

            #region 일일퀘스트 관련 데이터
            param.Add("Daily_Attendance", Data_Manager.Main_Players_Data.Daily_Attendance); // 일일퀘스트 출석
            param.Add("Daily_Levelup", Data_Manager.Main_Players_Data.Levelup); //일일퀘스트 레벨업 진행도 (24시 이후 초기화)
            param.Add("Daily_Summon", Data_Manager.Main_Players_Data.Summon); //일일퀘스트 영웅소환 진행도 (24시 이후 초기화)
            param.Add("Daily_Relic", Data_Manager.Main_Players_Data.Relic); //일일퀘스트 유물소환 진행도 (24시 이후 초기화)
            param.Add("Daily_Dungeon_Gold", Data_Manager.Main_Players_Data.Dungeon_Gold); //골드 던전 클리어 횟수 (24시 이후 초기화)
            param.Add("Daily_Dungeon_Dia", Data_Manager.Main_Players_Data.Dungeon_Dia); //다이아 던전 클리어 횟수 (24시 이후 초기화)

            param.Add("Daily_Attendance_Clear", Data_Manager.Main_Players_Data.Daily_Attendance_Clear); // 출석체크 클리어 여부
            param.Add("Daily_Levelup_Clear", Data_Manager.Main_Players_Data.Level_up_Clear); // 레벨업 클리어 여부
            param.Add("Daily_Summon_Clear", Data_Manager.Main_Players_Data.Summon_Clear); // 영웅소환 클리어 여부
            param.Add("Daily_Relic_Clear", Data_Manager.Main_Players_Data.Relic_Clear); // 유물소환 클리어 여부
            param.Add("Daily_Dungeon_Gold_Clear", Data_Manager.Main_Players_Data.Dungeon_Gold_Clear); // 골드던전 클리어 여부
            param.Add("Daily_Dungeon_Dia_Clear", Data_Manager.Main_Players_Data.Dungeon_Dia_Clear); // 다이아던전 클리어 여부
            param.Add("Fast_Mode", Data_Manager.Main_Players_Data.isFastMode); // 패스트모드 활성화 여부
            #endregion

            var bro = Backend.GameData.Get("USER", new Where());

            if (bro.IsSuccess())
            {
                var inDate = bro.GetInDate();

                Backend.Leaderboard.User.UpdateMyDataAndRefreshLeaderboard(Utils.LEADERBOARD_UUID, "USER", inDate, param, (callback) =>
                {
                    if (callback.IsSuccess())
                    {
                        Debug.Log("유저 기본 데이터 업데이트 및 리더보드 갱신에 성공하였습니다.");
                    }
                    else
                    {
                        Debug.LogWarning("유저 기본 데이터 업데이트 및 리더보드 갱신에 실패하였습니다.");

                        Backend.Leaderboard.User.GetMyLeaderboard(Utils.LEADERBOARD_UUID, callback => 
                        {
                            if (!callback.IsSuccess())
                            {
                                Debug.Log("리더보드에 등록되어 있지않습니다. 재등록 후 재 저장 시도합니다.");

                                var bro = Backend.GameData.GetMyData("USER", new Where());
                                string inDate = bro.GetInDate();
                                var _bro = Backend.URank.User.UpdateUserScore(Utils.LEADERBOARD_UUID, "USER", inDate, param); // 리더보드에 유저데이터를 등록합니다.

                                if (_bro.IsSuccess())
                                {
                                    Debug.Log($"리더보드 등록에 성공하였습니다. : {callback}");

                                }
                                else
                                {
                                    Debug.LogError($"리더보드 등록에 실패하였습니다. : {callback}");
                                    //TODO : 일단 업데이트만.
                                }
                            }

                            else
                            {
                                Debug.LogError($"리더보드 미등록 에러 외, 다른 원인의 에러가 있습니다. : {callback}");
                                return;
                            }


                        });
                     }
                });

            }

            else
            {
                return;
            }

            await Task.Yield();

            #endregion

            #region CHARACTER DATA

            Param character_param = new Param();
            string char_Json_Data = JsonConvert.SerializeObject(Base_Manager.Data.character_Holder);
            character_param.Add("character", char_Json_Data);


            Backend.GameData.Update("CHARACTER", new Where(), character_param, (callback) =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("유저 영웅 보유 데이터 업데이트에 성공하였습니다.");
                }
                else
                {
                    Debug.LogError("유저 영웅 보유 데이터 업데이트에 실패하였습니다.");
                }
            });

            await Task.Yield();

            #endregion

            #region ITEM_DATA
            Param item_param = new Param();
            string Json_item_Data = JsonConvert.SerializeObject(Base_Manager.Data.Item_Holder);
            item_param.Add("Item", Json_item_Data);

            Backend.GameData.Update("ITEM", new Where(), item_param, (callback) =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("유저 인벤토리 보유 데이터 업데이트에 성공하였습니다.");
                }
                else
                {
                    Debug.LogError("유저 인벤토리 보유 데이터 업데이트에 실패하였습니다.");
                }
            });


            #endregion

            #region SMELT_DATA
            Param smelt_param = new Param();
            string Json_Smelt_Data = JsonConvert.SerializeObject(Base_Manager.Data.User_Main_Data_Smelt_Array);
            smelt_param.Add("Smelt", Json_Smelt_Data);


            Backend.GameData.Update("SMELT", new Where(), smelt_param, (callback) =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("유저 각인 데이터 업데이트에 성공하였습니다.");
                }
                else
                {
                    Debug.LogError("유저 각인 데이터 업데이트에 실패하였습니다.");
                }
            });

            await Task.Yield();

            #endregion

            #region PLAYER_SET_HERO_DATA
            Param Player_Set_Hero_param = new Param();
            string Json_Player_Set_Hero_data = JsonConvert.SerializeObject(Base_Manager.Character.Set_Character);
            Player_Set_Hero_param.Add("Player_Set_Hero", Json_Player_Set_Hero_data);

            Debug.Log("유저 영웅 배치 데이터를 수정합니다");


            Backend.GameData.Update("PLAYER_SET_HERO", new Where(), Player_Set_Hero_param, (callback) =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("유저 영웅 배치 데이터 업데이트에 성공하였습니다.");
                }
                else
                {
                    Debug.LogError("유저 영웅 배치 데이터 업데이트에 실패하였습니다.");
                }
            });

            await Task.Yield();


            #endregion

            #region PLAYER_SET_RELIC_DATA
            Param Player_Set_Relic_Param = new Param();
            string Json_Player_Set_Relic_data = JsonConvert.SerializeObject(Base_Manager.Data.Main_Set_Item);
            Player_Set_Relic_Param.Add("Player_Set_Relic", Json_Player_Set_Relic_data);

            Debug.Log("유저 유물 배치 데이터를 수정합니다");

            Backend.GameData.Update("PLAYER_SET_RELIC", new Where(), Player_Set_Relic_Param, (callback) =>
            {
                if (callback.IsSuccess())
                {
                    Debug.Log("유저 유물 배치 데이터 업데이트에 성공하였습니다.");
                    Relic_Manager.instance.Initalize(); // 장착된 유물의 액티브효과를 적용시킵니다.
                }
                else
                {
                    Debug.LogError("유저 유물 배치 데이터 업데이트에 실패하였습니다.");
                }
            });

            await Task.Yield();

            #endregion
        }

        catch (Exception e)
        {
            Debug.LogError($"[ERROR] WriteDataAsync() 실행 중 예외 발생: {e.Message}");
        }
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

                }

                Data_Manager.Main_Players_Data = data;

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
