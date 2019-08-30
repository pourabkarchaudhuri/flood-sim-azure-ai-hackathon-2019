using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine.UI;



/// <summary>
/// Jaw Lip Sunc Class
/// </summary>

namespace CleverTalk.Controller{
	public class SimpleLipSyncJAW : MonoBehaviour {
		#region Private Member
		private float[] freqData;
		private int nSamples = 256;
		private int freqMax = 24000;
		private float volume = 20;
		private float LowF = 200;
		private float HighF = 400;
		private float JawPosX = 0;
		private float MoveBoneX = 0.0f;
		private float tempMoveBoneX = 0.0f;
		private int filterSize = 6;
		private float[] filter;
		private float fSummary;
		private int filterPosition = 0;
		private int SamplesSmooth = 0;

		#endregion

		#region Public Member

		public float JawDistance = 0.015f;
		public float AudioVolume = 100.0f;

		/// <summary>The number of samples to take at a time</summary>
		public const int NUM_SAMPLES = 1024;
		
		[HideInInspector]
		public bool isTalking = false;

		public InputField inpVolume;
		public InputField inpVolumeMultiplier;
		public InputField inpMoveBoneX0;
		public InputField inpMinJawOpeningRotation;

		public Toggle tglFixedUpdate;

		public static SimpleLipSyncJAW instance;

		public GameObject Jaw;
		public float inital_XRot;
		public InputField init_XrotField;
		#endregion


		/// <summary>
		/// these functions are declared in plugins/lipSync.jslib which gets the frequency samples from the web gl audio layer
		/// </summary>
		[DllImport("__Internal")]
		private static extern bool StartLipSampling(string name, float duration, int bufferSize);
		[DllImport("__Internal")]
		private static extern bool CloseLipSampling(string name);
		[DllImport("__Internal")]
		private static extern bool GetLipSamples(string name, float[] freqData, int size);

	
		#region MonoBehaviour Call Back

		void Awake()
		{
			inital_XRot = float.Parse (init_XrotField.text);
			instance = this;		
		}

		// Update is called once per frame
		void FixedUpdate () {
			if (isTalking == true && tglFixedUpdate.isOn ==  true)
			{
				//get the samples at this frame
				#if UNITY_WEBGL && !UNITY_EDITOR
				GetLipSamples("LipsyncJS", freqData, freqData.Length);
				#else
				//it don't work in unity webgl, for unity webgl we get the freqData from GetLipSamples
				AudioListener.GetSpectrumData(freqData, 0, FFTWindow.BlackmanHarris);
				#endif

				TalkMode();
			}
		}

		void Update(){


			if (isTalking == true && tglFixedUpdate.isOn ==  false)
			{

				#if UNITY_WEBGL && !UNITY_EDITOR
				//get the samples at this frame
				GetLipSamples("LipsyncJS", freqData, freqData.Length);
				#endif

				TalkMode();
			}
		}

		#endregion


		#region Public Methods

		/// <summary>
		/// Start talking process
		/// </summary>
		public void	StartTalking(){
			if(isTalking == true){
				StopTalking();
			}

			AudioSource audioSrc = this.GetComponent<AudioSource>();
			audioSrc.Play();

			Talk();
		}


		/// <summary>
		/// Gets the duration of the sound.
		/// </summary>
		/// <returns>The sound duration.</returns>
		public float getSoundDuration()
		{
			AudioSource audioSrc = this.GetComponent<AudioSource>();
			return audioSrc.clip.length;
		}

		/// <summary>
		/// Sets the volume.
		/// </summary>
		/// <param name="volumeValue">Volume value.</param>
		/// <param name="lowFreq">Low freq.</param>
		/// <param name="highFreq">High freq.</param>
		/// <param name="filterSize">Filter size.</param>
		public void SetVolume(float volumeValue, float lowFreq = 0, float highFreq=0, int filterSize = 6)
		{
			if (lowFreq == 0)
				lowFreq = 100.0f;
			if (highFreq == 0)
				highFreq = 300.0f;

			volume = volumeValue / 2;

			LowF = lowFreq;
			HighF = highFreq;
			filterSize = filterSize;
		}

		/// <summary>
		/// Smooths movements
		/// </summary>
		/// <returns>smooth move value</returns>
		/// <param name="sampleRate">Sample rate.</param>
		public float SmoothMove(float sampleRate)
		{
			if (SamplesSmooth == 0)
				filter = new float[filterSize];

			fSummary += sampleRate - filter[filterPosition];
			filter[filterPosition++] = sampleRate;

			if (filterPosition > SamplesSmooth)
				SamplesSmooth = filterPosition;

			filterPosition = filterPosition % filterSize;

			return fSummary / SamplesSmooth;
		}		

		public float SimulateTones(float freqLow, float freqHigh)
		{
			freqLow = Mathf.Clamp(freqLow, 20, freqMax);
			freqHigh = Mathf.Clamp(freqHigh, freqLow, freqMax);

			int t1 = (int) Mathf.Floor(freqLow * nSamples / freqMax);
			int t2 = (int) Mathf.Floor(freqHigh * nSamples / freqMax);

			float sum = 0;

			string strFreqData =  "";
			for (int i=t1; i <= t2; i++)
			{
				sum += freqData[i];	
				strFreqData += freqData[i].ToString() + "||";
			}

			return sum / (t2 - t1 + 1);
		}

		public void ValidMoveMaker()
		{
			float volumeMultiplierInput = float.Parse(inpVolumeMultiplier.text.Trim());

			float getValue = (SmoothMove(SimulateTones(LowF,HighF)) * volume * volumeMultiplierInput);

			MoveBoneX = getValue;
		}

		/// <summary>
		/// Talk mode
		/// </summary>
		public void TalkMode()
		{
			ValidMoveMaker();		

			if(MoveBoneX <= 0){
				MoveBoneX = float.Parse(inpMoveBoneX0.text.Trim());
			}

			float jawXRotationValue = MoveBoneX + inital_XRot;
			float minJawXRotation =  float.Parse(inpMinJawOpeningRotation.text.Trim());

			if(jawXRotationValue > minJawXRotation){
				jawXRotationValue = minJawXRotation;
			}

			SetJawXRotation (jawXRotationValue);
		}

		/// <summary>
		/// Stop talking.
		/// </summary>
		public void StopTalking(){
			isTalking = false;
			this.GetComponent<AudioSource>().Stop();
			CloseSampling();
			SetJawXRotation (inital_XRot);
		}

		public void initXRotValue()
		{
			inital_XRot = float.Parse (init_XrotField.text);

		}




		#endregion


		#region Private Methods

		void SetJawXRotation(float xValue)
		{
			Vector3 eularAngles = Jaw.transform.localEulerAngles;
			eularAngles.x = xValue;
			Jaw.transform.localEulerAngles = eularAngles;
		}

		void Talk()
		{
			freqData = new float[nSamples];

			if(inpVolume.text.Trim().Length > 0){
				SetVolume (float.Parse(inpVolume.text.Trim()));
			}else{
				SetVolume (AudioVolume);
			}

			#if UNITY_WEBGL && !UNITY_EDITOR
				StartCoroutine("WaitForStartLipSync");
				StartCoroutine("WaitForTime");
			#else				
				isTalking  = true;
			#endif
		}

		IEnumerator WaitForTime(){
			yield return new WaitForSeconds(1000f);
			if(isTalking == false){
				StopCoroutine("WaitForStartLipSync");
			}
		}

		IEnumerator WaitForStartLipSync(){		
			while(isTalking == false){
				isTalking = StartLipSampling("LipsyncJS", getSoundDuration(), NUM_SAMPLES);	
				yield return null;
			}

			StopCoroutine("WaitForTime");

		//	Debug.Log(isTalking);
		}


		/// <summary>Close on destroy</summary>
		private void OnDestroy() {
			CloseSampling();
		}

		void CloseSampling(){
			#if UNITY_WEBGL && !UNITY_EDITOR
			CloseLipSampling("LipsyncJS");
			#endif
		}
		#endregion







	}
}
