# Unityのアセットバンドル検証

Unity 5.4.2f2でアセットバンドルの作成、読み込みのテスト。
下記のstart.unityを開き、画面上部のメニュー「Assets」「Build Asset Bundles」を実行すると、AssetBundlesフォルダにアセットバンドルが作成される。


## ファイル構成
+ AssetBundles                # 生成されたAssetBundle保存先
  - AssetBundles              # UnityFS形式のバイナリ
  - AssetBundles.manifest     # 「black」「white」の記載あり
  - white                     # UnityFS形式のバイナリ
  - black                     # UnityFS形式のバイナリ
  - gray                      # UnityFS形式のバイナリ
  - white.manifest            # 「Assets/AssetBundles/Black/BlackBox.prefab」「Assets/AssetBundles/Black/Black.png」の記載あり
  - black.manifest            # 「Assets/AssetBundles/White/WhiteBox.prefab」「Assets/AssetBundles/White/White.png」の記載あり
  - gray.manifest             # 「Assets/AssetBundles/Gray/Gray.png」の記載あり
+ Unity
  + Assets
    - start.unity             # AssetBundle読み込みテストシーン
    + Editor
      - CreateAssetBundles.cs # AssetBundle作成プログラム
    + AssetBundles
      + White                 # AssetLabel「white」、中のprefabは「Gray/Gray.png」と「Share/Heavy.png」を参照
      + Black                 # AssetLabel「black」、中のprefabは「Gray/Gray.png」と「Share/Heavy.png」を参照
      + Gray                  # AssetLabel「gray」
      + Share                 # AssetLabel未設定

## 考察
- AssetLabelには小文字は可、大文字不可
- 「AssetBundles/white」と「AssetBundles/black」のファイルサイズが同一かつ1Mを超えているため、依存先のファイル「Share/Heavy」はコピーして重複保持される
- 「Black/BlackBox.prefab」のImage「Gray/Gray.png」への参照がMissing。依存先ファイルに別AssetLabelが付いていると参照が切れる
- WWWとAssetBundleRequestをyield returnで待たなくとも正常実行できるが、その場合はメインスレッドがブロックされる（PCで検証、AndroidやiPhoneでは未検証）
