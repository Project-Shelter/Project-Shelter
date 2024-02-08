# Project-Shelter

## ✈️ Project Convention

아래의 모든 컨벤션은 프로젝트 협업에 있어 가독성 및 협업의 편의를 위해 제시된 컨벤션입니다.
완전하게 지켜지지 않아도 상관없으며 이후 변경될 수도 있습니다.

### 1️⃣ Coding Convention

[c-sharp-style-guide](https://github.com/kodecocodes/c-sharp-style-guide) 에 제시된 C# Convention을 기준으로 작성합니다.

### 2️⃣ commit convention

커밋 메세지 컨벤션은 아래 표를 기준으로 `아이콘 요약 | 상세 내용` 으로 작성합니다. 예를 들어 캐릭터와 관련된 새로운 스크립트를 추가한 경우

🔨	 Add | Character Script 추가

✨ Feat | Character 움직임 관련 로직 추가

➕	 Add	| Character 내 UI Dependecy 추가

🐛 Fix	| Character 점프 시 이동이 불가능한 버그 수정

와 같이 커밋 메세지를 작성합니다. 커밋메세지는 협업에 있어 다른 사람과 본인이 쉽게 해당되는 커밋에 대해 알아볼 수 있게 하는 것을 목표로 합니다.

아이콘 | 설명 |	원문
|---|---|---|
🔨	|	개발 스크립트 추가/수정	| Add or update development scripts.
🎨	| 	코드의 구조/형태 개선 | 	Improve structure / format of the code.
⚡️	|	성능 개선	| Improve performance.
🔥	|	코드/파일 삭제 |	Remove code or files.
🐛	|	버그 수정	| Fix a bug.
🚑	|	긴급 수정 |	Critical hotfix.
✨	|	새 기능 |	Introduce new features.
📝	|	문서 추가/수정 |	Add or update documentation.
✅	 | 테스트 추가/수정	| Add or update tests.
🔒	| 보안 이슈 수정	| Fix security issues.
🔖	|	릴리즈/버전 태그 |	Release / Version tags.
📌	|	특정 버전 의존성 고정 |	Pin dependencies to specific versions.
📈	|	분석, 추적 코드 추가/수정	| Add or update analytics or track code.
♻️	 |	코드 리팩토링	| Refactor code.
➕	 | 의존성 추가	| Add a dependency.
➖	 |	의존성 제거 |	Remove a dependency.
🔧	|	구성 파일 추가/삭제 | 	Add or update configuration files.
🌐	|	국제화/현지화	| Internationalization and localization.
💩	|	똥싼 코드	| Write bad code that needs to be improved.
⏪	 |	변경 내용 되돌리기 |	Revert changes.
🔀	|	브랜치 합병 |	Merge branches.
📦	|	컴파일된 파일, 패키지 추가/수정 |	Add or update compiled files or packages.
👽	|	외부 API 변화로 인한 수정 |	Update code due to external API changes.
💡	|	주석 추가/수정	| Add or update comments in source code.
🗃	|	데이버베이스 관련 수정 |	Perform database related changes.
🔊	|	로그 추가/수정 |	Add or update logs.
🙈	|	.gitignore 추가/수정 |	Add or update a .gitignore file.

아트 및 리소스 관련 이모지
아이콘 | 설명 |	원문
|---|---|---|
🖌 |	UI/스타일 파일 추가/수정 |	Add or update the UI and style files.
🚚	|	리소스 이동, 이름 변경	| Move or rename resources (e.g.: files paths routes).

🔗 https://inpa.tistory.com/entry/GIT-%E2%9A%A1%EF%B8%8F-Gitmoji-%EC%82%AC%EC%9A%A9%EB%B2%95-Gitmoji-cli

### 3️⃣ pull request convention

풀 리퀘스트 컨벤션은 `[현재 브랜치 명] 해당 풀리퀘스트의 전반적인 내용` 으로 작성됩니다.

개발이 완료된 브랜치는 pull request를 통해 main으로 병합을 진행합니다.

### 4️⃣ file / Resource convention

Unity Scene 의 UI Object 의 경우 파스칼 케이스에 띄어쓰기의 경우 언더바를 사용하여 `[UI]_[Content]` 으로 작성합니다. 

(ex. Button_GameStartButton)

Localization Table 의 Key String의 경우도 UI 와 동일 하게 `Key_[Content]` 로 작성 합니다. 

(ex. Key_Start)
