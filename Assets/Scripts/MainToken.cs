using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        // Wait for the rotation animation to finish
        yield return new WaitForSeconds(1);
        animator.SetTrigger("Idle");

        // Continue with the rest of the logic
        if (!matched)
        {
            if (spriteRenderer.sprite == back)
            {
                if (!gameControl.GetComponent<GameControl>().TwoCardsUp())
                {
                    spriteRenderer.sprite = faces[faceIndex];
                    gameControl.GetComponent<GameControl>().AddVisibleFace(faceIndex);
                    if (gameControl.GetComponent<GameControl>().TwoCardsUp())
                    {
                       // RotateToken();
                        StartCoroutine(CheckMatchedCards());
                    }
                }
            }
            else
            {
                spriteRenderer.sprite = back;
                gameControl.GetComponent<GameControl>().RemoveVisibleFace(faceIndex);
            }
        }
    }

   

    IEnumerator CheckMatchedCards()
    {
        yield return new WaitForSeconds(1f); // Adjust delay time as needed
        animator.SetTrigger("Idle");

        if (!gameControl.GetComponent<GameControl>().CheckMatch())
        {
            // Cards are not matched, close only the newly opened unmatched cards
            foreach (var token in GameObject.FindGameObjectsWithTag("Token"))
            {
                var mainToken = token.GetComponent<MainToken>();
                if (!mainToken.matched && mainToken.spriteRenderer.sprite != mainToken.back)
                {
                   mainToken.RotateToken(); // Trigger closing animation
                    yield return new WaitForSeconds(1.2f);
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
        animator.SetTrigger("Rotate");
    }
}