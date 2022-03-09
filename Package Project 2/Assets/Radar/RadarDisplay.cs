using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class RadarDisplay : MonoBehaviour
{

    [SerializeField, Tooltip("Distance in length to edge of radar, in Metres")]
    private float scale = 10;
    private RectTransform radarTransform;
    private Vector2 centre;

    public List<GameObject> contacts;
    private List<GameObject> blips = new List<GameObject>();
    private List<GameObject> arrows = new List<GameObject>();

    [SerializeField]
    private GameObject blipPrefab;
    [SerializeField]
    private GameObject arrowPrefab;
    [SerializeField]
    private GameObject radarImage;

    public Slider slider;

    void Start()
    {
        slider.value = scale / 100;
        radarTransform = radarImage.GetComponent<RectTransform>();

        centre = new Vector2(radarTransform.localScale.x/2, radarTransform.localScale.y / 2);

        foreach (GameObject blip in contacts)
        {
            ping(blip.transform.position);
        }
        //Instantiate a blip on the radar for each radar contact
    }

    private void Update()//garboge that should be removed
    {
        scale = slider.value * 100;
    }

    public void ping(Vector3 pos)
    {
        Vector3 relativePos = pos - transform.position;
        //Debug.Log(relativePos);
        GameObject i_blip = Instantiate(blipPrefab, radarImage.transform);
        GameObject i_arrow = Instantiate(arrowPrefab, radarImage.transform.parent);
        blips.Add(i_blip);
        arrows.Add(i_arrow);
        i_arrow.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -11);// Arbitrary value based off uneven PNG
    }

    public void updPos(GameObject blip)
    {
        // Divide x and y size (in pixels) by scale float to get pixels per metre
        //float radScale = radarTransform.localScale.x * scale;
        float radScale = (radarTransform.sizeDelta.x * radarTransform.localScale.x)/scale;
        Vector2 radOffset = new Vector2(transform.position.x, transform.position.z)* radScale;

        if (blip.GetComponent<RadarContact>().index == -1)
        {
            blip.GetComponent<RadarContact>().index = contacts.IndexOf(blip);
        }
        else
        {
            Vector2 posOnScreen = (new Vector2(blip.transform.position.x, blip.transform.position.z)*radScale) - (radOffset);
            int index = blip.GetComponent<RadarContact>().index;
            GameObject selectedBlip = blips[index];
            GameObject selectedArrow = arrows[index];

            //Debug.Log($"{Vector3.Distance(blip.transform.position, transform.position)} is the distance to the player??");
            if (Vector3.Distance(blip.transform.position, transform.position) > scale/2)
            {
                selectedBlip.GetComponent<Image>().enabled = false;
                selectedArrow.SetActive(true);
            }
            else
            {
                selectedBlip.GetComponent<Image>().enabled = true;
                selectedArrow.SetActive(false);
            }

            //Debug.Log($"{blip.transform.position.x * radScale},{blip.transform.position.z * radScale}");


            //Convert from world space to radar space

            radarImage.GetComponent<RectTransform>().rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.y);
            selectedBlip.GetComponent<RectTransform>().anchoredPosition = posOnScreen;
            selectedArrow.transform.rotation = Quaternion.Euler(0, 0, Vector3.SignedAngle(contacts[index].transform.position - transform.position, transform.forward, Vector3.up));

        }

    }
}