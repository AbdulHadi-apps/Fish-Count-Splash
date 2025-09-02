using EasyTransition;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KidsManager : MonoBehaviour
{
    public TransitionSettings transition;
    public float startDelay;

    public GameObject NicoIdle;
    public GameObject NicoTalk;
    public GameObject LucasIdle;
    public GameObject LucasTalk;
    public GameObject RosieIdle;
    public GameObject RosieTalk;

    public ParticleSystem NicoParticle;
    public ParticleSystem LucasParticle;
    public ParticleSystem RosieParticle;

    public ParticleSystem NicoPuff;
    public ParticleSystem LucasPuff;
    public ParticleSystem RosiePuff;

    public AudioSource NicoAudio;
    public AudioSource LucasAudio;
    public AudioSource RosieAudio;

    public AudioSource BabyNico;
    public AudioSource BabyLucas;
    public AudioSource BabyRosie;

    private bool IsNicoTalking;
    private bool IsLucasTalking;
    private bool IsRosieTalking;

    private SpeechAnimator NicoSpeech;
    private SpeechAnimator LucasSpeech;
    private SpeechAnimator RosieSpeech;

    private float interval1;
    private float interval2;
    private float interval3;
    void Start()
    {
        NicoSpeech = NicoTalk.GetComponent<SpeechAnimator>();
        LucasSpeech = LucasTalk.GetComponent<SpeechAnimator>();
        RosieSpeech = RosieTalk.GetComponent<SpeechAnimator>();

        interval1 = NicoAudio.clip.length;
        interval2 = NicoAudio.clip.length + LucasAudio.clip.length-1;
        interval3 = NicoAudio.clip.length + LucasAudio.clip.length + RosieAudio.clip.length-2.5f;

        StartCoroutine(PlayAudioSequence());
    }

    private IEnumerator PlayAudioSequence()
    {
        NicoIdle.SetActive(false);
        NicoTalk.SetActive(true);
        NicoAudio.Play();
        NicoParticle.Play();
        yield return new WaitForSeconds(interval1);
        NicoPuff.Play();
        NicoIdle.SetActive(true);
        NicoTalk.SetActive(false);


        LucasIdle.SetActive(false);
        LucasTalk.SetActive(true);
        LucasParticle.Play();
        LucasAudio.Play();
        yield return new WaitForSeconds(interval2);
        LucasPuff.Play();
        LucasIdle.SetActive(true);
        LucasTalk.SetActive(false);


        RosieIdle.SetActive(false);
        RosieTalk.SetActive(true);
        RosieParticle.Play();
        RosieAudio.Play();
        yield return new WaitForSeconds(interval3);
        NicoIdle.SetActive(false);
        NicoTalk.SetActive(true);
        LucasIdle.SetActive(false);
        LucasTalk.SetActive(true);
        RosieIdle.SetActive(false);
        RosieTalk.SetActive(true);
        BabyNico.Play();
        BabyLucas.Play();
        BabyRosie.Play();
       
        yield return new WaitForSeconds(3f);
        ChangeScene();
    }

    void Update()
    {
        /*IsNicoTalking = NicoSpeech.isTalking;
        IsLucasTalking = LucasSpeech.isTalking;
        IsRosieTalking = RosieSpeech.isTalking;
        if (IsLucasTalking)
        {
            LucasIdle.SetActive(false);
            LucasTalk.SetActive(true);
        }
        else
        {
            LucasIdle.SetActive(true);
            LucasTalk.SetActive(false);
        }
        if (IsRosieTalking)
        {
            RosieIdle.SetActive(false);
            RosieTalk.SetActive(true);
        }
        else
        {
            RosieIdle.SetActive(true);
            RosieTalk.SetActive(false);
        }
        if (IsNicoTalking)
        {
            NicoIdle.SetActive(false);
            NicoTalk.SetActive(true);
        }
        else
        {
            NicoIdle.SetActive(true);
            NicoTalk.SetActive(false);
        }*/

    }

    void ChangeScene()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

        TransitionManager.Instance().Transition(SceneManager.GetActiveScene().buildIndex + 1, transition, startDelay);
    }
}
