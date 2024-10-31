## 유의할 점 및 팁
* 본 코드는 참고용입니다. 각자의 케이스에 따라 문제가 발생하는 이유가 다를 수 있습니다.
* Unity와 Arduino의 통신은 통신 데이터의 길이, 처리 속도 및 순서, 양측의 평균 및 최소 갱신율 등에 민감합니다.
* 각자의 secrets(WiFi 비밀번호, API Key 등)가 git에 올라가지 않도록 주의하세요.
* 시리얼 포트는 동시에 한 프로그램만 점유할 수 있습니다. Arduino IDE에서 serial monitor가 열려 있으면 Unity와의 통신이 불가능합니다. 반대로 Unity 코드가 실행중이라면 Arduino IDE에서 코드 upload가 불가능합니다.
