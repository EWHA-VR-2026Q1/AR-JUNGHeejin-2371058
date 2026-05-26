# AR-JUNGHeejin-2371058

이 저장소는 AR/VR 수업 과제 제출용 GitHub Repository입니다.  
각 과제는 과제 번호에 따라 브랜치 또는 메인 브랜치에 정리되어 있습니다.

## 제출자 정보

- 이름: 정희진
- 학번: 2371058
- 수업: AR/VR
- Repository: AR-JUNGHeejin-2371058

## 과제 제출 현황

| 과제 | 위치 | 설명 |
|---|---|---|
| HW19 | main 브랜치 | HW19 과제 파일 |
| HW20 | HW20_LimitedBody 브랜치 | Limited Body 관련 Unity 프로젝트 |
| HW21 | main 브랜치 | Unity Canvas Render Mode 비교 과제 |
| HW23 | main 브랜치 | 플레이어 및 월드 Transform 상태 저장/불러오기 과제 |

## 브랜치 안내

### main 브랜치

`main` 브랜치에는 HW19, HW21, HW23 과제가 업로드되어 있습니다.

### HW20_LimitedBody 브랜치

HW20 과제는 별도 브랜치에 업로드되어 있습니다.

- 브랜치 이름: `HW20_LimitedBody`
- GitHub 링크:  
  https://github.com/EWHA-VR-2026Q1/AR-JUNGHeejin-2371058/tree/HW20_LimitedBody

## HW20 확인 방법

1. GitHub 저장소 상단의 브랜치 선택 메뉴를 클릭합니다.
2. `HW20_LimitedBody` 브랜치를 선택합니다.
3. 해당 브랜치에서 HW20 Unity 프로젝트 파일을 확인할 수 있습니다.

또는 아래 링크로 바로 이동할 수 있습니다.

https://github.com/EWHA-VR-2026Q1/AR-JUNGHeejin-2371058/tree/HW20_LimitedBody

## HW21 확인 방법

HW21 과제 씬은 다음 경로에 있습니다.

- `Assets/HW21_Canvas/HW21_Canvas.unity`

해당 씬은 Unity Canvas의 Render Mode 3가지를 비교하기 위한 실습 씬입니다.

씬에는 다음 Canvas 3개가 포함되어 있습니다.

| Canvas 이름 | Render Mode | 설명 |
|---|---|---|
| Canvas_01_ScreenSpace_Overlay | Screen Space - Overlay | 화면 위에 고정되는 UI |
| Canvas_02_ScreenSpace_Camera | Screen Space - Camera | ARCamera를 Render Camera로 연결한 UI |
| Canvas_03_WorldSpace | World Space | 3D 공간 안에 배치된 UI |

실행 시에는 Hierarchy에서 확인할 Canvas 하나만 활성화하고, 나머지 두 Canvas는 비활성화한 뒤 Play 버튼을 누릅니다.  
각 Canvas에는 Text와 Button이 있으며, 버튼 클릭 시 Text 내용이 변경됩니다.

또한 Game 화면에서 마우스 커서를 움직이면 3D 오브젝트가 회전하여 Canvas Render Mode별 UI 표시 차이를 확인할 수 있습니다.

## HW23 확인 방법

HW23 과제 씬은 다음 경로에 있습니다.

- `Assets/HW23/HW23_DataSaveLoad/HW23_DataSaveLoad.unity`

해당 씬은 플레이어와 월드 오브젝트의 Transform 상태를 저장하고 불러오는 실습 씬입니다.  
플레이어 1개와 오브젝트 2개, 총 3개 이상의 Transform을 저장 대상으로 사용합니다.

저장 대상은 `WorldSaveLoadManager`의 `Save Targets` 리스트에 등록되어 있으며, 단일 변수 방식이 아닌 `List` 기반 구조로 관리됩니다.

저장되는 Transform 정보는 다음과 같습니다.

| 저장 대상 | 저장 정보 |
|---|---|
| Player | Position, Rotation |
| SaveObject_A | Position, Rotation |
| SaveObject_B | Position, Rotation |

저장 데이터는 `WorldData`, `TransformData` 클래스를 사용해 구성되며, JSON 형식으로 저장됩니다.  
저장 경로는 Unity의 `Application.persistentDataPath`를 사용합니다.

실행 방법은 다음과 같습니다.

1. Unity에서 `Assets/HW23/HW23_DataSaveLoad/HW23_DataSaveLoad.unity` 씬을 엽니다.
2. Play 버튼을 눌러 실행합니다.
3. `W/A/S/D` 키로 Player를 이동합니다.
4. `Q/E` 키로 Player를 회전합니다.
5. Scene 뷰에서 `SaveObject_A`, `SaveObject_B`의 위치 또는 회전을 변경합니다.
6. 화면의 `Save` 버튼을 눌러 현재 상태를 저장합니다.
7. Player와 오브젝트들의 위치를 다시 변경합니다.
8. 화면의 `Load` 버튼을 누르면 저장했던 위치와 회전으로 복원됩니다.
9. Play Mode를 종료한 뒤 다시 실행하면 이전에 저장한 상태가 자동으로 복원됩니다.

키보드 단축키는 다음과 같습니다.

| 키 | 기능 |
|---|---|
| W/A/S/D | Player 이동 |
| Q/E | Player 회전 |
| F5 | 저장 |
| F9 | 불러오기 |

Player 오브젝트에는 회전 상태를 쉽게 확인할 수 있도록 한쪽 옆면에 작은 원형 표시가 붙어 있습니다.  
이를 통해 저장 후 불러오기 시 Position뿐 아니라 Rotation도 함께 복원되는 것을 확인할 수 있습니다.
