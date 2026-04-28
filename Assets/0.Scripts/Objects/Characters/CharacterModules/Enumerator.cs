//경험치
public enum ExperienceValue
{
    //농사, 낚시, 제작, 광질
    Farming, Fishing, Production, MineralQuality, 
    Length
}

//움직임
public enum MoveCheckType
{
    //멈춤, 점프, 걷기, 상호작용, 
    Idle, Jump, Walk, Rotate,
    Length
}

//상태
public enum Conditions
{
    //배고픔, 갈증, 환각, 
    Hunger, Thirst, Hallucination, 
    Length
}

//등급
public enum Grade
{
    //가죽, 철, 골드, 다이아몬드, 이리듐
    Normal, Silver, Gold, Diamond, Iridium,
    Length
}

//아이템 타입
public enum ItemCategory
{
    Tool,           // 도구
    Equipment,      // 의복
    BasicResource,  // 기본자원
    ProcessedResource, // 가공자원
    Gem,            // 보석
    Mineral,        // 광물
    Seed,           // 씨앗
    Crop,           // 작물
    Livestock,      // 축산
    Herb,           // 약초
    HealResource,   // 회복자원
    Ingredient,     // 요리재료
    Food,           // 요리
    Book,           // 책
    Structure,      // 시설물
    Hallucinogen,   // 환각제
    Record,         // 기록
    Misc,            // 기타
    Length
}
