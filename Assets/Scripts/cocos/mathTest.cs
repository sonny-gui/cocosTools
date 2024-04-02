using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mathTest : MonoBehaviour
{
    public InputField t1;
    public InputField t2;  
    public InputField t3;
    public InputField t4;
    public InputField t8;

    public Text t5;
    public Text t6;
    public Text t7;

    public InputField mult1;
    public InputField mult2;
    public InputField mult3;
    public InputField mult4;
    public InputField mult5;

    public InputField lineCount;

   

    public void Start()
    {
        mult1.text = "2";
        mult2.text = "4";
        mult3.text = "6";
        mult4.text = "8";
        mult5.text = "10";
    }

    public void Update()
    {
        if (string.IsNullOrEmpty(t1.text) || string.IsNullOrEmpty(t2.text) || string.IsNullOrEmpty(t3.text) || string.IsNullOrEmpty(t8.text) || string.IsNullOrEmpty(lineCount.text))
        {
            return;
        }
        float scale = float.Parse(t3.text);
        float total = float.Parse(t1.text);
        float nowMult = float.Parse(t2.text);
        float baseMult = nowMult;
        //if (mult1.transform.parent.gameObject.activeSelf)
        //    nowMult += float.Parse(mult1.text);
        //if (mult2.transform.parent.gameObject.activeSelf)
        //    nowMult += float.Parse(mult2.text);
        //if (mult3.transform.parent.gameObject.activeSelf)
        //    nowMult += float.Parse(mult3.text);
        //if (mult4.transform.parent.gameObject.activeSelf)
        //    nowMult += float.Parse(mult4.text);
        //if (mult5.transform.parent.gameObject.activeSelf)
        //    nowMult += float.Parse(mult5.text);
        float getMult = total / (nowMult * scale);
        t4.text = getMult.ToString();

        switch (lineCount.text)
        {
            case "1":
                mult1.transform.parent.gameObject.SetActive(true);
                mult1.transform.parent.gameObject.SetActive(true);
                mult2.transform.parent.gameObject.SetActive(false);
                mult3.transform.parent.gameObject.SetActive(false);
                mult4.transform.parent.gameObject.SetActive(false);
                mult5.transform.parent.gameObject.SetActive(false);
                mult5.transform.parent.gameObject.SetActive(false);
                t8.transform.parent.gameObject.SetActive(false);
                this.Mult1Func(getMult, baseMult);
                break;
            case "2":
                mult1.transform.parent.gameObject.SetActive(true);
                mult2.transform.parent.gameObject.SetActive(true);
                mult3.transform.parent.gameObject.SetActive(false);
                mult4.transform.parent.gameObject.SetActive(false);
                mult5.transform.parent.gameObject.SetActive(false);
                t8.transform.parent.gameObject.SetActive(true);
                this.Mult2Func(getMult, baseMult);
                break;
            case "3":
                mult1.transform.parent.gameObject.SetActive(true);
                mult2.transform.parent.gameObject.SetActive(true);
                mult3.transform.parent.gameObject.SetActive(true);
                mult4.transform.parent.gameObject.SetActive(false);
                mult5.transform.parent.gameObject.SetActive(false);
                t8.transform.parent.gameObject.SetActive(true);
                this.Mult3Func(getMult, baseMult);
                break;
            case "4":
                mult1.transform.parent.gameObject.SetActive(true);
                mult2.transform.parent.gameObject.SetActive(true);
                mult3.transform.parent.gameObject.SetActive(true);
                mult4.transform.parent.gameObject.SetActive(true);
                mult5.transform.parent.gameObject.SetActive(false);
                t8.transform.parent.gameObject.SetActive(true);
                this.Mult4Func(getMult, baseMult);
                break;
            case "5":
                mult1.transform.parent.gameObject.SetActive(true);
                mult2.transform.parent.gameObject.SetActive(true);
                mult3.transform.parent.gameObject.SetActive(true);
                mult4.transform.parent.gameObject.SetActive(true);
                mult5.transform.parent.gameObject.SetActive(true);
                t8.transform.parent.gameObject.SetActive(true);
                this.Mult5Func(getMult, baseMult);
                break;
            default:
                return;
        }
    }

    void Mult5Func(float getMult,float nowMult)
    {
        float mult_1 = float.Parse(this.mult1.text);
        mult_1 = mult_1 <= 0 ? 1 : mult_1;
        float getLine_1 = getMult / float.Parse(t8.text);
        getMult = getMult - getLine_1;
        float line_1 = getLine_1 / mult_1;

        float mult_2 = float.Parse(this.mult2.text);
        mult_2 = mult_2 <= 0 ? 1 : mult_2;
        float getLine_2 = getMult / float.Parse(t8.text);
        getMult = getMult - getLine_2;
        float line_2 = getLine_2 / mult_2;

        float mult_3 = float.Parse(this.mult3.text);
        mult_3 = mult_3 <= 0 ? 1 : mult_3;
        float getLine_3 = getMult / float.Parse(t8.text);
        getMult = getMult - getLine_3;
        float line_3 = getLine_3 / mult_3;

        float mult_4 = float.Parse(this.mult4.text);
        mult_4 = mult_4 <= 0 ? 1 : mult_4;
        float getLine_4 = getMult / float.Parse(t8.text);
        getMult = getMult - getLine_4;
        float line_4 = getLine_4 / mult_4;

        float mult_5 = float.Parse(this.mult5.text);
        mult_5 = mult_5 <= 0 ? 1 : mult_5;
        float getLine_5 = getMult;
        float line_5 = getLine_5 / mult_5;


        float getLineMult = line_1 * mult_1 + line_2 * mult_2 + line_3 * mult_3 + line_4 * mult_4 + line_5 * mult_5;
        t5.text = line_1 + "x" + mult_1 + "+" + line_2 + "x" + mult_2 + "+" + line_3 + "x" + mult_3 + "+" + line_4 + "x" + mult_4 + "+" + line_5 + "x" + mult_5 + "=" + getLineMult;


        mult_1 = mult_1 == 1 ? 0 : mult_1;
        mult_2 = mult_2 == 1 ? 0 : mult_2;
        mult_3 = mult_3 == 1 ? 0 : mult_3;
        mult_4 = mult_4 == 1 ? 0 : mult_4;
        mult_5 = mult_5 == 1 ? 0 : mult_5;

        float finLintMult = line_1 * (mult_1 + nowMult) + line_2 * (mult_2 + nowMult) + line_3 * (mult_3 + nowMult) + line_4 * (mult_4 + nowMult) + line_5 * (mult_5 + nowMult);
        t6.text = line_1 + "x" + (mult_1 + nowMult) + "+" + line_2 + "x" + (mult_2 + nowMult) + "+" + line_3 + "x" + (mult_3 + nowMult) + "+" + line_4 + "x" + (mult_4 + nowMult) + "+" + line_5 + "x" + (mult_5 + nowMult) + "=" + finLintMult;
        t7.text = finLintMult.ToString();
    }

    void Mult4Func(float getMult, float nowMult)
    {
        float mult_1 = float.Parse(this.mult1.text);
        mult_1 = mult_1 <= 0 ? 1 : mult_1;
        float getLine_1 = getMult / float.Parse(t8.text);
        getMult = getMult - getLine_1;
        float line_1 = getLine_1 / mult_1;

        float mult_2 = float.Parse(this.mult2.text);
        mult_2 = mult_2 <= 0 ? 1 : mult_2;
        float getLine_2 = getMult / float.Parse(t8.text);
        getMult = getMult - getLine_2;
        float line_2 = getLine_2 / mult_2;

        float mult_3 = float.Parse(this.mult3.text);
        mult_3 = mult_3 <= 0 ? 1 : mult_3;
        float getLine_3 = getMult / float.Parse(t8.text);
        getMult = getMult - getLine_3;
        float line_3 = getLine_3 / mult_3;

        float mult_4 = float.Parse(this.mult4.text);
        mult_4 = mult_4 <= 0 ? 1 : mult_4;
        float getLine_4 = getMult;
        float line_4 = getLine_4 / mult_4;

        float getLineMult = line_1 * mult_1 + line_2 * mult_2 + line_3 * mult_3 + line_4 * mult_4;
        t5.text = line_1 + "x" + mult_1 + "+" + line_2 + "x" + mult_2 + "+" + line_3 + "x" + mult_3 + "+" + line_4 + "x" + mult_4 + "+" + "=" + getLineMult;


        mult_1 = mult_1 == 1 ? 0 : mult_1;
        mult_2 = mult_2 == 1 ? 0 : mult_2;
        mult_3 = mult_3 == 1 ? 0 : mult_3;
        mult_4 = mult_4 == 1 ? 0 : mult_4;

        float finLintMult = line_1 * (mult_1 + nowMult) + line_2 * (mult_2 + nowMult) + line_3 * (mult_3 + nowMult) + line_4 * (mult_4 + nowMult);
        t6.text = line_1 + "x" + (mult_1 + nowMult) + "+" + line_2 + "x" + (mult_2 + nowMult) + "+" + line_3 + "x" + (mult_3 + nowMult) + "+" + line_4 + "x" + (mult_4 + nowMult) + "+" + "=" + finLintMult;
        t7.text = finLintMult.ToString();
    }

    void Mult3Func(float getMult, float nowMult)
    {
        float mult_1 = float.Parse(this.mult1.text);
        mult_1 = mult_1 <= 0 ? 1 : mult_1;
        float getLine_1 = getMult / float.Parse(t8.text);
        getMult = getMult - getLine_1;
        float line_1 = getLine_1 / mult_1;

        float mult_2 = float.Parse(this.mult2.text);
        mult_2 = mult_2 <= 0 ? 1 : mult_2;
        float getLine_2 = getMult / float.Parse(t8.text);
        getMult = getMult - getLine_2;
        float line_2 = getLine_2 / mult_2;

        float mult_3 = float.Parse(this.mult3.text);
        mult_3 = mult_3 <= 0 ? 1 : mult_3;
        float getLine_3 = getMult;
        float line_3 = getLine_3 / mult_3;


        float getLineMult = line_1 * mult_1 + line_2 * mult_2 + line_3 * mult_3;
        t5.text = line_1 + "x" + mult_1 + "+" + line_2 + "x" + mult_2 + "+" + line_3 + "x" + mult_3 + "=" + getLineMult;


        mult_1 = mult_1 == 1 ? 0 : mult_1;
        mult_2 = mult_2 == 1 ? 0 : mult_2;
        mult_3 = mult_3 == 1 ? 0 : mult_3;

        float finLintMult = line_1 * (mult_1 + nowMult) + line_2 * (mult_2 + nowMult) + line_3 * (mult_3 + nowMult);
        t6.text = line_1 + "x" + (mult_1 + nowMult) + "+" + line_2 + "x" + (mult_2 + nowMult) + "+" + line_3 + "x" + (mult_3 + nowMult) + "+" + "=" + finLintMult;
        t7.text = finLintMult.ToString();
    }

    void Mult2Func(float getMult, float nowMult)
    {
        float mult_1 = float.Parse(this.mult1.text);
        mult_1 = mult_1 <= 0 ? 1 : mult_1;
        float getLine_1 = getMult / float.Parse(t8.text);
        getMult = getMult - getLine_1;
        float line_1 = getLine_1 / mult_1;

        float mult_2 = float.Parse(this.mult2.text);
        mult_2 = mult_2 <= 0 ? 1 : mult_2;
        float getLine_2 = getMult;
        float line_2 = getLine_2 / mult_2;


        float getLineMult = line_1 * mult_1 + line_2 * mult_2;
        t5.text = line_1 + "x" + mult_1 + "+" + line_2 + "x" + mult_2 + "+" + "=" + getLineMult;


        mult_1 = mult_1 == 1 ? 0 : mult_1;
        mult_2 = mult_2 == 1 ? 0 : mult_2;

        float finLintMult = line_1 * (mult_1 + nowMult) + line_2 * (mult_2 + nowMult);
        t6.text = line_1 + "x" + (mult_1 + nowMult) + "+" + line_2 + "x" + (mult_2 + nowMult) + "+" + "=" + finLintMult;
        t7.text = finLintMult.ToString();
    }

    void Mult1Func(float getMult, float baseMult)
    {
        float mult_1 = float.Parse(this.mult1.text);
        float line_1 = getMult;


        float getLineMult = line_1 * mult_1 ;
        t5.text = line_1 + "x" + mult_1 + "=" + getLineMult;

        mult_1 = mult_1 == 1 ? 0 : mult_1;

        float finLintMult = line_1 * (mult_1 + baseMult);
        t6.text = line_1 + "x" + (mult_1 + baseMult) + "=" + finLintMult;
        t7.text = finLintMult.ToString();
    }
}

