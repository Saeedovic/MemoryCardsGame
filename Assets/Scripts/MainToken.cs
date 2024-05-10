using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MainToken : MonoBehaviour
{
    GameObject gameControl;
    SpriteRenderer spriteRenderer;
    public Sprite[] faces;
    public Sprite back;
    public int faceIndex;
    public bool matched = false;
    private Animator animator;
    // public GameControl _gameControl;

    void Start()
    {
        gameControl = GameObject.Find("GameControl");
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    public void OnMouseDown()
    {
        if (!gameControl.GetComponent<GameControl>().TwoCardsUp())
        {
            //. RotateToken();
            StartCoroutine(ProcessMouseDown());
        }
    }

    IEnumerator ProcessMouseDown()
    {
        /* AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
              animator.SetTrigger("Flip");
              yield return new WaitForSecondsRealtime(stateInfo.length * 0.5f);*/

        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        if (!matched)
        {
            if (spriteRenderer.sprite == back)
            {
                if (!gameControl.GetComponent<GameControl>().TwoCardsUp())
                {
                    animator.SetTrigger("Flip");

                    yield return new WaitForSecondsRealtime(stateInfo.length * 0.5f);
                    Debug.Log(" ROTATION 1 = " + transform.rotation.eulerAngles.y);
                    spriteRenderer.sprite = faces[faceIndex];
                    Debug.Log("ROTATION 2 = " + transform.rotation.eulerAngles.y);



                    //  transform.rotation = Quaternion.Euler(0, 180, 0);


                    animator.SetTrigger("Idle");
                    Debug.Log(" ROTATION 3 = " + transform.rotation.eulerAngles.y);






                    // yield return new WaitForSecondsRealtime(stateInfo.length);
                    gameControl.GetComponent<GameControl>().AddVisibleFace(faceIndex);
                    


                    // Play the flip animation forward
                    //    animator.SetTrigger("Flip");

                    if (gameControl.GetComponent<GameControl>().TwoCardsUp())
                    {
                        yield return new WaitForSecondsRealtime(stateInfo.length);
                        StartCoroutine(CheckMatchedCards());
                    }
                }
            }
            else
            {
                animator.SetTrigger("Flip");
                yield return new WaitForSecondsRealtime(stateInfo.length * 0.5f);
                spriteRenderer.sprite = back;
                gameControl.GetComponent<GameControl>().RemoveVisibleFace(faceIndex);


            }
        }
        Debug.Log(" ROTATION 4 = " + transform.rotation.eulerAngles.y);

    }




    IEnumerator CheckMatchedCards()
    {

        animator.SetTrigger("Idle");

        if (!gameControl.GetComponent<GameControl>().CheckMatch())
        {

            foreach (var token in GameObject.FindGameObjectsWithTag("Token"))
            {
                var mainToken = token.GetComponent<MainToken>();
                if (!mainToken.matched && mainToken.spriteRenderer.sprite != mainToken.back)
                {
                    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                    // animator.SetTrigger("Flip");
                    Debug.Log("Flip2");





                    animator.SetTrigger("Flip");
                    yield return new WaitForSecondsRealtime(stateInfo.length * 0.5f);
                    mainToken.spriteRenderer.sprite = mainToken.back;
                    gameControl.GetComponent<GameControl>().RemoveVisibleFace(mainToken.faceIndex);
                }
            }
        }
        else
        {

            foreach (var token in GameObject.FindGameObjectsWithTag("Token"))
            {
                var mainToken = token.GetComponent<MainToken>();
                if (!mainToken.matched && mainToken.spriteRenderer.sprite != mainToken.back)
                {
                    mainToken.matched = true;
                }
            }
        }
    }


    public void RotateToken()
    {
        // animator.SetTrigger("Rotate");
    }
}