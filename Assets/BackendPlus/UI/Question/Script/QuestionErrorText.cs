using LitJson;

namespace BackendPlus.UI.Question {
    public class QuestionErrorText {
        public string firstKeyError;
        public string tryError;
        public string normalError;
        public string loadingTimeOutError;

        public QuestionErrorText(JsonData textJson) {
            firstKeyError       = textJson["firstKeyError"].ToString();
            tryError            = textJson["tryError"].ToString();
            normalError         = textJson["normalError"].ToString();
            loadingTimeOutError = textJson["loadingTimeOutError"].ToString();
        }

        public QuestionErrorText() {
            firstKeyError = "더 이상 문의를 불러올 수 없습니다.";
            tryError = "잠시 후 다시 시도해주세요";
            normalError = "{0}중 에러가 발생했습니다.";
            loadingTimeOutError = "요청 시간이 초과하였습니다. 잠시 후 다시 시도해주세요.";
        }
    }
}