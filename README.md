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
| HW21 | main 브랜치 | HW21 과제 파일 |

## 브랜치 안내

### main 브랜치

`main` 브랜치에는 HW19와 HW21 과제가 업로드되어 있습니다.

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
