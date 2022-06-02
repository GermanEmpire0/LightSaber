using BepInEx;
using System;
using UnityEngine;
using Utilla;
using UnityEngine.XR;
using System.IO;
using System.Reflection;

namespace LightSaber
{
    /// <summary>
    /// This is your mod's main class.
    /// </summary>

    /* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
    [ModdedGamemode]
    [BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        bool inRoom;

        private readonly XRNode rNode = XRNode.RightHand;

        private readonly XRNode lNode = XRNode.RightHand;

        GameObject _LightSaber;

        GameObject _StarWarsSong;

        bool isgrip;

        bool cangrip = true;

        GameObject HandR;

        bool istrigger;

        bool cantrigger = true;

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

        void OnGameInitialized(object sender, EventArgs e)
        {
            Stream _str = Assembly.GetExecutingAssembly().GetManifestResourceStream("LightSaber.Assets.starwarssong");
            AssetBundle _bundle = AssetBundle.LoadFromStream(_str);
            GameObject StarWarsGameObject = _bundle.LoadAsset<GameObject>("StarWarsSong");
            _StarWarsSong = Instantiate(StarWarsGameObject);
            
            Stream str = Assembly.GetExecutingAssembly().GetManifestResourceStream("LightSaber.Assets.lightsaber");
            AssetBundle bundle = AssetBundle.LoadFromStream(str);
            GameObject LightSaberGameObject = bundle.LoadAsset<GameObject>("LightSaber");
            _LightSaber = Instantiate(LightSaberGameObject);

            HandR = GameObject.Find("OfflineVRRig/Actual Gorilla/rig/body/shoulder.R/upper_arm.R/forearm.R/hand.R/palm.01.R/");
            _LightSaber.transform.SetParent(HandR.transform, false);
            _LightSaber.transform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
            _LightSaber.transform.localRotation = Quaternion.Euler(0f, 180f, 90f);
            _LightSaber.transform.localPosition = new Vector3(-0.3f, 0.5f, 0f);
        }

        void Update()
        {
            InputDevices.GetDeviceAtXRNode(rNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out isgrip);
            InputDevices.GetDeviceAtXRNode(lNode).TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out istrigger);

            if (isgrip)
            {
                if (cangrip)
                {
                    _StarWarsSong.GetComponent<AudioSource>().Play();
                    cangrip = false;
                }
                else
                {
                    cangrip = true;
                }
            }

            if (istrigger)
            {
                if (cantrigger)
                {
                    _StarWarsSong.GetComponent<AudioSource>().Stop();
                    cantrigger = false;
                }
                else
                {
                    cantrigger = true;
                }
            }
        }

        /* This attribute tells Utilla to call this method when a modded room is joined */
        [ModdedGamemodeJoin]
        public void OnJoin(string gamemode)
        {
            /* Activate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = true;
        }

        /* This attribute tells Utilla to call this method when a modded room is left */
        [ModdedGamemodeLeave]
        public void OnLeave(string gamemode)
        {
            /* Deactivate your mod here */
            /* This code will run regardless of if the mod is enabled*/

            inRoom = false;
        }
    }
}
