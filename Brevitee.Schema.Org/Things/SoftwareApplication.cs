using System;

namespace Brevitee.Schema.Org
{
	///<summary>A software application.</summary>
	public class SoftwareApplication: CreativeWork
	{
		///<summary>Type of software application, e.g. "Game, Multimedia".</summary>
		public ThisOrThat<Text, URL> ApplicationCategory {get; set;}
		///<summary>Subcategory of the application, e.g. "Arcade Game".</summary>
		public ThisOrThat<Text, URL> ApplicationSubCategory {get; set;}
		///<summary>The name of the application suite to which the application belongs (e.g. Excel belongs to Office)</summary>
		public Text ApplicationSuite {get; set;}
		///<summary>Countries for which the application is not supported. You can also provide the two-letter ISO 3166-1 alpha-2 country code.</summary>
		public Text CountriesNotSupported {get; set;}
		///<summary>Countries for which the application is supported. You can also provide the two-letter ISO 3166-1 alpha-2 country code.</summary>
		public Text CountriesSupported {get; set;}
		///<summary>Device required to run the application. Used in cases where a specific make/model is required to run the application.</summary>
		public Text Device {get; set;}
		///<summary>If the file can be downloaded, URL to download the binary.</summary>
		public URL DownloadUrl {get; set;}
		///<summary>Features or modules provided by this application (and possibly required by other applications).</summary>
		public ThisOrThat<Text, URL> FeatureList {get; set;}
		///<summary>MIME format of the binary (e.g. application/zip).</summary>
		public Text FileFormat {get; set;}
		///<summary>Size of the application / package (e.g. 18MB). In the absence of a unit (MB, KB etc.), KB will be assumed.</summary>
		public Integer FileSize {get; set;}
		///<summary>URL at which the app may be installed, if different from the URL of the item.</summary>
		public URL InstallUrl {get; set;}
		///<summary>Minimum memory requirements.</summary>
		public ThisOrThat<Text, URL> MemoryRequirements {get; set;}
		///<summary>Operating systems supported (Windows 7, OSX 10.6, Android 1.6).</summary>
		public Text OperatingSystem {get; set;}
		///<summary>Permission(s) required to run the app (for example, a mobile app may require full internet access or may run only on wifi).</summary>
		public Text Permissions {get; set;}
		///<summary>Processor architecture required to run the application (e.g. IA64).</summary>
		public Text ProcessorRequirements {get; set;}
		///<summary>Description of what changed in this version.</summary>
		public ThisOrThat<Text, URL> ReleaseNotes {get; set;}
		///<summary>Component dependency requirements for application. This includes runtime environments and shared libraries that are not included in the application distribution package, but required to run the application (Examples: DirectX, Java or .NET runtime).</summary>
		public ThisOrThat<Text, URL> Requirements {get; set;}
		///<summary>A link to a screenshot image of the app.</summary>
		public ThisOrThat<ImageObject, URL> Screenshot {get; set;}
		///<summary>Version of the software instance.</summary>
		public Text SoftwareVersion {get; set;}
		///<summary>Storage requirements (free space required).</summary>
		public ThisOrThat<Text, URL> StorageRequirements {get; set;}
	}
}
