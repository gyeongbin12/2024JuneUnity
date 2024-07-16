using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : Singleton<SceneController>
{
    [SerializeField] Image loadImage;
    
    public IEnumerator FadeIn()
    {
        Color color =  loadImage.color;

        color.a = 1;

        loadImage.gameObject.SetActive(true);

        while (color.a >= 0.0f)
        {
            color.a -= Time.deltaTime;

            loadImage.color = color;

            yield return null;
        }

        loadImage.gameObject.SetActive(false);
    }

    public IEnumerator AsyncLoad(int index)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(index);

        loadImage.gameObject.SetActive(true);

        asyncOperation.allowSceneActivation = false;

        Color color = loadImage.color;

        color.a = 0;

        while (asyncOperation.isDone == false)
        {
            color.a += Time.deltaTime;

            loadImage.color = color;
            
            if(asyncOperation.progress > 0.9f)
            {
                color.a = Mathf.Lerp(color.a, 1f, Time.deltaTime);

                loadImage.color = color;

                if(color.a >= 1.0f)
                {
                    asyncOperation.allowSceneActivation = false;

                    yield break;
                }

            }

            yield return null;
        }

        //  bool allowSceneActivation : ����� �غ�Ǵ� ��� ����� Ȱ��ȭ��ų �� ����
        //                              ��� ���θ� �Ǵ��ϴ� �Լ�

        // bool isDone : �ش� ������ �غ� �Ǿ��� �� Ȯ���ϴ� �Լ� 

        // float progress : �۾��� ���� ������ 0 �� 1 ������ ������ Ȯ���ϴ� �Լ�
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FadeIn());
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
