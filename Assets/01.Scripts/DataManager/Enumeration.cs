using System;

[Serializable]
public enum EVALUATION_TYPE
{
    TRAIL_MARKING = 0,              // 주의력 평가 - 선긋기
    TEXT_BUTTON_PRESS,              // 주의력 평가 - 텍스트 버튼 누르기
    NUM_BUTTON_PRESS,               // 주의력 평가 - 숫자 버튼 누르기
    NUM_BUTTON_PRESS_REVERSE,       // 주의력 평가 - 숫자 버튼 역순 누르기
    FIND_OVERLAP_PICTURE,           // 지각능력 평가 - 겹친그림찾기
    PICTURE_PUZZLE,                 // 지각능력 평가 - 퍼즐그림 재구성
    PEGBOARD,                       // 지각능력 평가 - 페그보드
    STACK_BLOCK,                    // 지각능력 평가 - 블록쌓기
    FIND_OBJECT,                    // 지연회상 평가 - 숨긴 물건 찾기
    MEMORIZE_OBJECT,                // 지연회상 평가 - 물건 기억하기
    GROUPING_OBJECT,                // 지연회상 평가 - 물건 범주화하기
    SORT_OBJECT                     // 지연회상 평가 - 그림 순서화하기
}

[Serializable]
public enum TRAINING_TYPE
{
    DRESS_CHOICE = 0,
    PICTURE_PUZZLE,
    YABAWEE,
    COIN_BANK,
}

[Serializable]
public enum DATA_TYPE
{
    CLIENT,
    MANAGER
}
