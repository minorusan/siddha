using System;
using System.Collections;
using System.Collections.Generic;
using Core.Characters.Player;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class Inhale : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private enum EInhaleStatus
    {
        None,
        Inhaled,
        Exhaling,
        Exhaled
    }

    private AudioSource _source;
    private Color _from;
    private float _timePassed;

    private EInhaleStatus _state;
    private float _penalty;
    private Image _blowImage;
    private Image _selfImage;

    private bool _inhaled;
    private bool _shouldCancelPenalty;
    private float _originalCameraSize;

    public float MaxInhaleTime;
    public float PenaltyTime;
    public Image Status;

    public AudioClip InhaleSound;
    public AudioClip GaspSound;
    public AudioClip ExhaleSound;
    public Camera[] Cameras;
    private float _velocity;

    void Start()
    {
        Status.gameObject.SetActive(false);
        _blowImage = transform.GetChild(0).GetComponent<Image>();
        _selfImage = GetComponent<Image>();
        PenaltyTime = GaspSound.length;
        _source = GetComponent<AudioSource>();
        _originalCameraSize = Camera.main.orthographicSize;
    }

    void Update()
    {
        HandlePenaltyIfNeeded();

        switch (_state)
        {
            case EInhaleStatus.Inhaled:
                {
                    _timePassed += Time.unscaledDeltaTime;

                    if (_timePassed <= MaxInhaleTime)
                    {
                        Status.fillAmount = _timePassed / MaxInhaleTime;
                     
                        _source.volume = _timePassed / MaxInhaleTime;
                        for (int i = 0; i < Cameras.Length; i++)
                        {
                            Cameras[i].orthographicSize = Mathf.SmoothDamp(Cameras[i].orthographicSize,
                                _originalCameraSize * 0.5f, ref _velocity, MaxInhaleTime);
                        }
                        Status.color = Color.Lerp(Color.green, Color.red, Status.fillAmount);
                    }
                    else
                    {
                        _state = EInhaleStatus.Exhaling;
                        PenaltyTime = GaspSound.length;
                        _penalty = PenaltyTime;
                        _shouldCancelPenalty = false;
                      
                        AudioSource.PlayClipAtPoint(GaspSound, Camera.main.transform.position, 1f);
                       
                    }
                    break;
                }
            case EInhaleStatus.Exhaling:
                {
                    _from = Status.color;
                    PlayerQuirks.StoppedBreathing = false;
                    _state = EInhaleStatus.Exhaled;
                    StartCoroutine(Exhale());
                    break;
                }
        }
    }

    private void HandlePenaltyIfNeeded()
    {
        if (_penalty > 0f)
        {
            _penalty -= Time.deltaTime;
            _selfImage.fillAmount = 1f - _penalty / PenaltyTime;
            _blowImage.color = new Color(1f, 1f, 1f, _selfImage.fillAmount);
            _source.volume = _penalty / PenaltyTime;

            if (_penalty <= 0f)
            {
                _source.Stop();
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_state == EInhaleStatus.None && _penalty <= 0f)
        {
            _source.Play();
            _shouldCancelPenalty = true;
            AudioSource.PlayClipAtPoint(InhaleSound, Camera.main.transform.position, 0.2f);
            Debug.Log("Camera size" + _originalCameraSize);
            PlayerQuirks.StoppedBreathing = true;
            _state = EInhaleStatus.Inhaled;
            StopAllCoroutines();
            Status.gameObject.SetActive(true);
           
            _timePassed = 0f;
           
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _state = EInhaleStatus.Exhaling;
        _inhaled = false;
        AudioSource.PlayClipAtPoint(ExhaleSound, Camera.main.transform.position, Status.fillAmount);
        PlayerBehaviour.CurrentPlayer.Noise = Status.fillAmount / 2f;
        PlayerQuirks.StoppedBreathing = false;
        if (_shouldCancelPenalty)
        {
            _penalty = _timePassed * 0.3f;
            PenaltyTime = _timePassed * 0.3f;
        }
    }

    private IEnumerator Exhale()
    {
        while (Status.fillAmount > 0f && Math.Abs(Cameras[0].orthographicSize - _originalCameraSize) > 0.0000001f)
        {
            Status.fillAmount -= 0.01f;
            Status.color = Color.Lerp(_from, Color.green, 1f - Status.fillAmount);
            if (_penalty > 10f)
            {
                PlayerBehaviour.CurrentPlayer.Noise = Status.fillAmount;
            }

           
            for (int i = 0; i < Cameras.Length; i++)
            {
                Cameras[i].orthographicSize = Mathf.SmoothDamp(Cameras[i].orthographicSize,
                    _originalCameraSize, ref _velocity, 0.3f);
            }
            yield return new WaitForEndOfFrame();

        }
        if (_penalty <= 0f)
        {
            _source.Stop();
        }
        _state = EInhaleStatus.None;
        Status.gameObject.SetActive(false);
    }
}
