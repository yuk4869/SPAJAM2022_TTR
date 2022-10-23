using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // <--忘れがち

public class ScoreScript : MonoBehaviour
{
    public Image gauge; // ゲージ本体画像をインスペクターからセット
    private float totalScore = 0f; // 得点を代入する変数

    private void Start()
    {
        StartCoroutine(AddScore(1f));
    }

    IEnumerator AddScore(float frame)
    {
        while (true) 
        {
            gauge.fillAmount = totalScore / 100.0f;
            float rnd = Random.Range(-5.5f, 9.9f);

            totalScore += rnd;
            yield return new WaitForSeconds(frame);
            // totalScoreを増減させるコードは今回は省略

        }


    }
}