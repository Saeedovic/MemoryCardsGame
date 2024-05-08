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
        RotateToken();
        StartCoroutine(ProcessMouseDown());
    }

    IEnumerator ProcessMouseDown()
    {
        // Get the current animator state info
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        animator.SetTrigger("Flip");
       // Debug.Log("Flip1");


        // Wait for the rotation animation to finish
        yield return new WaitForSecondsRealtime(stateInfo.length);

        // Continue with the rest of the logic
        if (!matched)
        {
            if (spriteRenderer.sprite == back)
            {
                if (!gameControl.GetComponent<GameControl>().TwoCardsUp())
                {
                    spriteRenderer.sprite = faces[faceIndex];
                    gameControl.GetComponent<GameControl>().AddVisibleFace(faceIndex);

                    // Play the flip animation forward
                //    animator.SetTrigger("Flip");

                    if (gameControl.GetComponent<GameControl>().TwoCardsUp())
                    {
                        StartCoroutine(CheckMatchedCards());
                    }
                }
            }
            else
            {
                spriteRenderer.sprite = back;
                gameControl.GetComponent<GameControl>().RemoveVisibleFace(faceIndex);

                // Play the flip animation backward
               // animator.SetTrigger("Flip");
            }
        }
    }



    IEnumerator CheckMatchedCards()
    {
      //  yield return new WaitForSeconds(1f); // Adjust delay time as needed
      //  animator.SetTrigger("Idle");

        if (!gameControl.GetComponent<GameControl>().CheckMatch())
        {
            // Cards are not matched, close only the newly opened unmatched cards
            foreach (var token in GameObject.FindGameObjectsWithTag("Token"))
            {
                var mainToken = token.GetComponent<MainToken>();
                if (!mainToken.matched && mainToken.spriteRenderer.sprite != mainToken.back)
                {
                    AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
                    animator.SetTrigger("Flip");
                    Debug.Log("Flip2");




                    yield return new WaitForSecondsRealtime(stateInfo.length);
                    mainToken.spriteRenderer.sprite = mainToken.back;
                    gameControl.GetComponent<GameControl>().RemoveVisibleFace(mainToken.faceIndex);
                }
            }
        }
        else
        {
            // Cards are matched, set matched flag to true
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