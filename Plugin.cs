using BepInEx;
using System;
using UnityEngine;
using Utilla;
using UnityEngine.XR;
using System.IO;
using System.Reflection;
using System.ComponentModel;

namespace DarthVaderLightSaber
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [Description("HauntedModMenu")]
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;

        GameObject _StarWars;

        GameObject _LightSaber;

        GameObject HandR;

        GameObject LightSaberOn;

        GameObject LightSaberOff;

        GameObject LightSaberBlade;

        GameObject LightSaberBuzzingSounds;

        private readonly XRNode rNode = XRNode.RightHand;

        private readonly XRNode lNode = XRNode.RightHand;

        private readonly XRNode kNode = XRNode.RightHand;

        private readonly XRNode fNode = XRNode.RightHand;

        bool isgrip;

        bool cangrip = true;

        bool istrigger;

        bool cantrigger = true;

        bool isbutton;

        bool canbutton = true;

        bool _isbutton;

        bool _canbutton = true;

        void OnGameInitialized(object sender, EventArgs e)
        {
            Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("DarthVaderLightSaber.Assets.starwarssong");
            AssetBundle bundle = AssetBundle.LoadFromStream(str);
            GameObject StarWarsSoundObject = bundle.LoadAsset<GameObject>("StarWarsSong");
            _StarWars = Instantiate(StarWarsSoundObject);

            Stream _str = Assembly.GetExecutingAssembly().GetManifestResourceStream("DarthVaderLightSaber.Assets.lightsaber");
            AssetBundle _bundle = AssetBundle.LoadFromStream(_str);
            GameObject LightSaberObject = _bundle.LoadAsset<GameObject>("LightSaber");
            _LightSaber = Instantiate(LightSaberObject);

            HandR = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R/");
            _LightSaber.transform.SetParent(HandR.transform, false);
            _LightSaber.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
            _LightSaber.transform.localRotation = Quaternion.Euler(62.5278f, 260.205f, 85.5954f);
            _LightSaber.transform.localPosition = new Vector3(-0.05f, -0f, -0.02f);

            LightSaberBlade = GameObject.Find("Darth Vader Blade");

            Stream __str = Assembly.GetExecutingAssembly().GetManifestResourceStream("DarthVaderLightSaber.Assets.lightsaberon");
            AssetBundle __bundle = AssetBundle.LoadFromStream(__str);
            GameObject LightSaberIgnite = __bundle.LoadAsset<GameObject>("LightSaberIgnition");
            LightSaberOn = Instantiate(LightSaberIgnite);

            Stream ___str = Assembly.GetExecutingAssembly().GetManifestResourceStream("DarthVaderLightSaber.Assets.lightsaberoff");
            AssetBundle ___bundle = AssetBundle.LoadFromStream(___str);
            GameObject LightSaberStop = ___bundle.LoadAsset<GameObject>("LightSaberTurnOff");
            LightSaberOff = Instantiate(LightSaberStop);

            Stream ____str = Assembly.GetExecutingAssembly().GetManifestResourceStream("DarthVaderLightSaber.Assets.lightsaberbuzz");
            AssetBundle ____bundle = AssetBundle.LoadFromStream(____str);
            GameObject LightSaberBuzzing = ____bundle.LoadAsset<GameObject>("LightSaberBuzzSound");
            LightSaberBuzzingSounds = Instantiate(LightSaberBuzzing);
        }

        void OnEnable()
        {
            HarmonyPatches.ApplyHarmonyPatches();
            Utilla.Events.GameInitialized += OnGameInitialized;
        }

        void OnDisable()
        {
            HarmonyPatches.RemoveHarmonyPatches();
            Utilla.Events.GameInitialized -= OnGameInitialized;
        }

        void Update()
        {
            InputDevices.GetDeviceAtXRNode(rNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out isgrip);
            InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out istrigger);
            InputDevices.GetDeviceAtXRNode(kNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out isbutton);
            InputDevices.GetDeviceAtXRNode(fNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out _isbutton);

            if (isgrip)
            {
                // This is so we cannot spam the noise
                if (cangrip)
                {
                    _StarWars.GetComponent<AudioSource>().Play();
                    cangrip = false;
                }
            }
            else
            {
                // This is where grip is not pressed so here we will make it so u can grip
                cangrip = true;
            }

            if (istrigger)
            {
                if (cantrigger)
                {
                    _StarWars.GetComponent<AudioSource>().Stop();
                    cantrigger = false;
                }
                else
                {
                    cantrigger = true;
                }
            }
            
            if (isbutton)
            {
                if (canbutton)
                {
                    LightSaberOn.GetComponent<AudioSource>().Play();
                    LightSaberBlade.SetActive(true);
                    canbutton = false;
                }
                else
                {
                    LightSaberBlade.SetActive(false);
                    canbutton = true;
                }
            }

            if (_isbutton)
            {
                if (_canbutton)
                {
                    LightSaberOff.GetComponent<AudioSource>().Play();
                    LightSaberBlade.SetActive(false);
                    _canbutton = false;
                }
                else
                {
                    LightSaberBlade.SetActive(true);
                    _canbutton = true;
                }
            }

            if (LightSaberBlade.activeInHierarchy == false)
            {
                LightSaberBuzzingSounds.GetComponent<AudioSource>().Play();
            }
        }


        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            inRoom = true;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            inRoom = false;
        }
    }
}
