# WalkingAR
[Yu-Cheng Chang](https://github.com/Malik705017), [Yen-Pu Wang](https://github.com/paullongtan), [Chiao-Ju Chang](https://github.com/bwyanyuuu), [Wei Tian Mireille Tan](https://github.com/Mireille-T), [Yu Lun Hsu](https://github.com/YuLunHsu0912), [Yu Chen](https://github.com/chenyutpe), [Mike Y. Chen](https://mikechen.com/)

This repository is the opensource project for MobileHCI'24 paper: "Experience from Designing Augmented Reality Browsing Interfaces for Real-world Walking Scenarios"

![hero](https://github.com/user-attachments/assets/8acab01d-8b1e-431e-ba17-e94b4702b12f)

This paper presents the first exploration of AR browsing interface design and extended usage while walking in the wild. To achieve this goal, we developed a system providing easy-to-use browsing and UI adjustment interaction to facilitate
UI design exploration and refinement. Our system was developed using Unity and consists of two key modules: 1) the browsing interface module and 2) UI adjustment module. 

For the browsing module, we implemented a browser window using [Vuplex](https://developer.vuplex.com/webview/overview), which enables connection to commercial applications (e.g. web browsers). The interaction was implemented with the pinch and raycast interactors provided by the [Oculus Interaction SDK](https://developer.oculus.com/documentation/unity/unity-isdk-interaction-sdk-overview/), to enable clicking and scrolling of content using hand gestures, respectively.

For the UI adjustment module, we defined an additional set of hand gestures using the [hand pose detection](https://developer.oculus.com/documentation/unity/unity-isdk-hand-pose-detection/) provided by the SDK as well as AR panels to allow window parameters adjustment.
