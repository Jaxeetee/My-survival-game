using UnityEngine;


public class DelegateTesting : MonoBehaviour
{
    public delegate void OnVoidCall();

    public delegate float OnFloatCall(float num);

    public delegate void OnVoidParam(int num, string name);

    public void FloatTesting(OnFloatCall onFloatCall, float num)
    {
        float ans = onFloatCall.Invoke(num) + 1;

        Debug.Log($"ans is {ans}");
    }

    public void VoidWithParam(OnVoidParam onVoidParam)
    {
        Debug.Log("Done Declaring");
        onVoidParam(1,"dayum"); // in this case, these are the values of someInt and someString respectively
    }

    public void TestingDelegateAsParam(OnVoidCall callSomething)
    {
        callSomething?.Invoke();
    }

    // private void Start() {
        
    //     TestingDelegateAsParam(() => {
    //         Debug.Log("Detected Something");
    //     });

    //     TestingDelegateAsParam(() => {
    //         float add = 1 + 1;
    //         Debug.Log($"{add}");
    //     });


    //     //Return value
    //     FloatTesting(x =>
    //     {   
    //         Debug.Log(x);
    //         if (x < 4) return 3;
    //         else return 5;
    //     }, 2.0f);
    //     int one = 24;
    //     string j = "j";

    //     OnVoidParam onVoid = (someInt, someString) => 
    //     {   
    //         Debug.Log($"int: {someInt} somestring: {someString} num: {one} name:{j} using void with param");
    //     };

    //     VoidWithParam(onVoid);
    // }

}
