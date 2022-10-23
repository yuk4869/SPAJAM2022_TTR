using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <--忘れがち

public class ScoreScript : MonoBehaviour
{
    public Image gauge; // ゲージ本体画像をインスペクターからセット
    private int totalScore; // 得点を代入する変数

    void Update()
    {
        gauge.fillAmount = totalScore / 100.0f;
　　　　 // totalScoreを増減させるコードは今回は省略
    }
}