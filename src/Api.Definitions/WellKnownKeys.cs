#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2019                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Definitions
{
	/// <summary>
	/// Static class with well known attribute keys.
	/// </summary>
	public static class WellKnownKeys
	{
		#region class Catalog

		/// <summary>Well known keys for accessing value attributes.</summary>
		public static class Catalog
		{
			#region constants

			/// <summary>Direction</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Direction = 2009;

			/// <summary>Orientation</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Orientation = 2090;

			/// <summary>Location</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Location = 2091;

			/// <summary>Lower class limit</summary>
			/// <remarks>Float</remarks>
			public const ushort LowerClassLimitKey = 2135;

			/// <summary>Upper class limit</summary>
			/// <remarks>Float</remarks>
			public const ushort UpperClassLimitKey = 2136;

			/// <summary>Reference system</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort ReferenceSystem = 2520;

			/// <summary>Color scheme position</summary>
			/// <remarks>Integer</remarks>
			public const ushort ColorSchemePositionKey = 2902;

			/// <summary>Status color</summary>
			/// <remarks>Integer</remarks>
			public const ushort StatusColorKey = 2903;

			/// <summary>Machine number</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort MachineNumber = 4062;

			/// <summary>Machine name</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort MachineName = 4063;

			/// <summary>Gage number</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort GageNumber = 4072;

			/// <summary>Gage name</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort GageName = 4073;

			/// <summary>Gage group</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort GageGroup = 4074;

			/// <summary>Operator number</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort OperatorNumber = 4092;

			/// <summary>Operator name</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort OperatorName = 4093;

			/// <summary>Result</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Result = 4230;

			/// <summary>Cavity number</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort CavityNumber = 4252;

			/// <summary>Cavity name</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort CavityName = 4253;

			/// <summary>Group type</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort GroupType = 4300;

			/// <summary>Approval</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Approval = 4320;

			/// <summary>Reason for test</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Reason = 4391;

			/// <summary>Key for the distribution type.</summary>
			/// <remarks>Integer</remarks>
			public const ushort DistributionTypeKey = 4403;

			/// <summary>Distribution type.</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort DistributionType = 4404;

			/// <summary>Long-term measurement</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort LongTermMeasurement = 4411;

			/// <summary>Yes/No</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort YesNo = 4527;

			/// <summary>Characteristic type</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort CharacteristicType = 12013;

			/// <summary>Limit type</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort LimitType = 12120;

			/// <summary>The key for the measured quantity type.</summary>
			/// <remarks>Integer</remarks>
			public const ushort MeasuredQuantityTypeKey = 13267;

			/// <summary>The description of the pda sample type.</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort PdaSampleTypeDescription = 13270;

			/// <summary>The key for the pda sample type.</summary>
			/// <remarks>Integer</remarks>
			public const ushort PdaSampleTypeKey = 13271;

			/// <summary>The description for the distribution analysis mode.</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort DistributionAnalysisModeDescription = 13280;

			/// <summary>The key for the distribution analysis mode.</summary>
			/// <remarks>Integer</remarks>
			public const ushort DistributionAnalysisModeKey = 13281;

			#endregion
		}

		#endregion

		#region class Characteristic

		/// <summary>Well known keys for accessing part characteristic.</summary>
		public static class Characteristic
		{
			#region constants

			/// <summary>Characteristic number</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Number = 2001;

			/// <summary>Characteristic description</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Description = 2002;

			/// <summary>Characteristic abbreviation</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Abbreviation = 2003;

			/// <summary>Characteristic direction</summary>
			/// <remarks>Catalog</remarks>
			public const ushort Direction = 2004;

			/// <summary>Documented flag</summary>
			/// <remarks>Catalog</remarks>
			public const ushort ControlItem = 2006;

			/// <summary>Characteristic group type</summary>
			/// <remarks>Catalog</remarks>
			public const ushort GroupType = 2008;

			/// <summary>Distribution type</summary>
			/// <remarks>Catalog</remarks>
			public const ushort DistributionType = 2011;

			/// <summary>The formula that describes a calculated characteristics</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort LogicalOperationString = 2021;

			/// <summary>Number decimal places</summary>
			/// <remarks>Integer</remarks>
			public const ushort DecimalPlaces = 2022;

			/// <summary>Characteristic status</summary>
			/// <remarks>Integer</remarks>
			public const ushort CharacteristicStatus = 2080;

			/// <summary>
			/// This is a descriptive text for the <see cref="CallbackUri"/>.
			/// </summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort CallbackUriText = 2097;

			/// <summary>
			/// The uri that can be used to create an interactive hyperlink that calls another application. This is used by PiWeb Monitor 
			/// for example to switch back to the measuring application (Calypso, Caligo etc.) when clicking on a characteristic or part.
			/// </summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort CallbackUri = 2098;

			/// <summary>Desired value</summary>
			/// <remarks>Float</remarks>
			public const ushort DesiredValue = 2100;

			/// <summary>Nominal value</summary>
			/// <remarks>Float</remarks>
			public const ushort NominalValue = 2101;

			/// <summary>Lower specification limit</summary>
			/// <remarks>Float</remarks>
			public const ushort LowerSpecificationLimit = 2110;

			/// <summary>Upper specification limit</summary>
			/// <remarks>Float</remarks>
			public const ushort UpperSpecificationLimit = 2111;

			/// <summary>Lower tolerance limit</summary>
			/// <remarks>Float</remarks>
			public const ushort LowerTolerance = 2112;

			/// <summary>Upper tolerance limit</summary>
			/// <remarks>Float</remarks>
			public const ushort UpperTolerance = 2113;

			/// <summary>Lower scrap limit</summary>
			/// <remarks>Float</remarks>
			public const ushort LowerScrapLimit = 2114;

			/// <summary>Upper scrap limit</summary>
			/// <remarks>Float</remarks>
			public const ushort UpperScrapLimit = 2115;

			/// <summary>Lower natural boundary</summary>
			/// <remarks>Catalog</remarks>
			public const ushort LowerNaturallyBoundary = 2120;

			/// <summary>Upper natural boundary</summary>
			/// <remarks>Catalog</remarks>
			public const ushort UpperNaturallyBoundary = 2121;

			/// <summary>The unit (mm, inch, °, etc.)</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Unit = 2142;

			/// <summary>Type of alignment (RPS, 321, ...)</summary>
			/// <remarks>Catalog</remarks>
			public const ushort AlignmentType = 2297;

			/// <summary>Position of a characteristic (left; right)</summary>
			/// <remarks>Catalog</remarks>
			public const ushort Position = 2298;

			/// <summary>Orientation of a characteristic (X; Y; Z; ...)</summary>
			/// <remarks>Catalog</remarks>
			public const ushort Orientation = 2299;

			/// <summary>Name of inspection plan</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort MeasurementModule = 2342;

			/// <summary>Name of the alignment (Ausrichtung_RPS, ...)</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort AlignmentName = 2511;

			/// <summary>Normal vector, x coordinate</summary>
			/// <remarks>Float</remarks>
			public const ushort I = 2540;

			/// <summary>Normal vector, y coordinate</summary>
			/// <remarks>Float</remarks>
			public const ushort J = 2541;

			/// <summary>Normal vector, z coordinate</summary>
			/// <remarks>Float</remarks>
			public const ushort K = 2542;

			/// <summary>Position, x coordinate</summary>
			/// <remarks>Float</remarks>
			public const ushort X = 2543;

			/// <summary>Position, y coordinate</summary>
			/// <remarks>Float</remarks>
			public const ushort Y = 2544;

			/// <summary>Position, z coordinate</summary>
			/// <remarks>Float</remarks>
			public const ushort Z = 2545;

			/// <summary>Diameter</summary>
			/// <remarks>Float</remarks>
			public const ushort Diameter = 2546;

			/// <summary>Length</summary>
			/// <remarks>Float</remarks>
			public const ushort Length = 2547;

			/// <summary>Length2</summary>
			/// <remarks>Float</remarks>
			public const ushort Length2 = 2548;

			/// <summary>Angle</summary>
			/// <remarks>Float</remarks>
			public const ushort Angle = 2549;

			/// <summary>Layer</summary>
			/// <remarks>Integer</remarks>
			public const ushort Layer = 2555;

			/// <summary>Untere Eingriffgrenze</summary>
			/// <remarks>Float</remarks>
			public const ushort LowerControlLimit = 8012;

			/// <summary>Obere Eingriffgrenze</summary>
			/// <remarks>Float</remarks>
			public const ushort UpperControlLimit = 8013;

			/// <summary>Untere Warngrenze</summary>
			/// <remarks>Float</remarks>
			public const ushort LowerWarningLimit = 8014;

			/// <summary>Obere Warngrenze</summary>
			/// <remarks>Float</remarks>
			public const ushort UpperWarningLimit = 8015;

			/// <summary>The sample size for a process distribution analysis.</summary>
			/// <remarks>Integer</remarks>
			public const ushort PdaSampleSize = 8500;

			/// <summary>
			/// Identifying different creation types of multi-samples for process distribution analysis.
			/// </summary>
			/// <remarks>Integer</remarks>
			public const ushort PdaSampleType = 8501;

			/// <summary>Merkmalsart (variabel; attributiv und attributiv zählend)</summary>
			/// <remarks>Catalog</remarks>
			public const ushort CharacteristicType = 12004;

			/// <summary>Messwertkatalog zur Auswertung attributiver Merkmale</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort MeasurementValueCatalog = 12005;

			/// <summary>Messgroesse (fuer Formplot)</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort PlotMeasurand = 12009;

			/// <summary>Typ des Merkmals (Circle; Cone; Plane; ...)</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort CharacteristicSpecification = 12010;

			/// <summary>Measured quantity type</summary>
			/// <remarks>Catalog</remarks>
			public const ushort MeasuredQuantityType = 13266;

			/// <summary>Identifying the different modes for distribution analysis.</summary>
			/// <remarks>Catalog</remarks>
			public const ushort DistributionAnalysisMode = 13282;

			/// <summary>Regelkartenkonfiguration der Lagekarte</summary>
			/// <remarks>AlphaNumeric</remarks>>
			public const ushort LocationChartConfiguration = 9010;

			/// <summary>Mittellinie der Lagekarte</summary>
			/// <remarks>Float</remarks>
			public const ushort LocationChartAverageValue = 9011;

			/// <summary>Untere Eingriffgrenze der Lagekarte</summary>
			/// <remarks>Float</remarks>
			public const ushort LocationChartLowerControlLimit = 9012;

			/// <summary>Obere Eingriffgrenze der Lagekarte</summary>
			/// <remarks>Float</remarks>
			public const ushort LocationChartUpperControlLimit = 9013;

			/// <summary>Untere Warngrenze der Lagekarte</summary>
			/// <remarks>Float</remarks>
			public const ushort LocationChartLowerWarningLimit = 9014;

			/// <summary>Obere Warngrenze der Lagekarte</summary>
			/// <remarks>Float</remarks>
			public const ushort LocationChartUpperWarningLimit = 9015;

			/// <summary>Regelkartenkonfiguration der Streuungskarte</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort VariationChartConfiguration = 9110;

			/// <summary>Mittellinie der Streuungskarte</summary>
			/// <remarks>Float</remarks>
			public const ushort VariationChartAverageValue = 9111;

			/// <summary>Untere Eingriffgrenze der Streuungskarte</summary>
			/// <remarks>Float</remarks>
			public const ushort VariationChartLowerControlLimit = 9112;

			/// <summary>Obere Eingriffgrenze der Streuungskarte</summary>
			/// <remarks>Float</remarks>
			public const ushort VariationChartUpperControlLimit = 9113;

			/// <summary>Untere Warngrenze der Streuungskarte</summary>
			/// <remarks>Float</remarks>
			public const ushort VariationChartLowerWarningLimit = 9114;

			/// <summary>Obere Warngrenze der Streuungskarte</summary>
			/// <remarks>Float</remarks>
			public const ushort VariationChartUpperWarningLimit = 9115;

			/// <summary>Stamp text</summary>
			/// <remarks>Integer</remarks>
			public const ushort HasStamp = 2808;

			/// <summary>Stamp text</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort StampCaption = 2809;

			/// <summary>Stamp position, x coordinate relative to bitmap</summary>
			/// <remarks>Float</remarks>
			public const ushort StampPositionX = 2810;

			/// <summary>Stamp position, y coordinate relative to bitmap</summary>
			/// <remarks>Float</remarks>
			public const ushort StampPositionY = 2811;

			/// <summary>Stamp target, x coordinate relative to bitmap</summary>
			/// <remarks>Float</remarks>
			public const ushort StampTargetX = 2812;

			/// <summary>Stamp target, y coordinate relative to bitmap</summary>
			/// <remarks>Float</remarks>
			public const ushort StampTargetY = 2813;

			/// <summary>Stamp radius relative to the bitmap width</summary>
			/// <remarks>Float</remarks>
			public const ushort StampRadius = 2814;

			#endregion
		}

		#endregion

		#region class Measurement

		/// <summary>Well known keys for accessing measurement attributes.</summary>
		public static class Measurement
		{
			#region constants

			/// <summary>Time</summary>
			/// <remarks>Date</remarks>
			public const ushort Time = 4;

			/// <summary>Event Id</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort EventId = 5;

			/// <summary>Batch number</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort BatchNumber = 6;

			/// <summary>Inspector name</summary>
			/// <remarks>Catalog</remarks>
			public const ushort InspectorName = 8;

			/// <summary>Comment</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Comment = 9;

			/// <summary>Machine number</summary>
			/// <remarks>Catalog</remarks>
			public const ushort MachineNumber = 10;

			/// <summary>Process Id</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort ProcessId = 11;

			/// <summary>Inspection equipment</summary>
			/// <remarks>Catalog</remarks>
			public const ushort InspectionEquipment = 12;

			/// <summary>Process value</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort ProcessValue = 13;

			/// <summary>Part Id</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort PartsId = 14;

			/// <summary>Inspection type</summary>
			/// <remarks>Catalog</remarks>
			public const ushort InspectionType = 15;

			/// <summary>Inspection number</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort ProductionNumber = 16;

			/// <summary>Contract</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Contract = 53;

			/// <summary>Measurement status (approved; blocked; ...)</summary>
			/// <remarks>Catalog</remarks>
			public const ushort MeasurementStatus = 96;

			/// <summary>Measurement change date</summary>
			/// <remarks>Date</remarks>
			public const ushort MeasurementChangeDate = 97;

			/// <summary>Measurement changed by</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort MeasurementChangedBy = 98;

			/// <summary>Contains the uuid of the aggregation job that created this measurement. Empty if this measurement is not an aggregated measurement.</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort AggregationJobUuid = 99;

			/// <summary>Contains the aggregation interval that was used to create this aggregated measurement.</summary>
			/// <remarks>Catalog</remarks>
			public const ushort AggregationInterval = 100;

			/// <summary>Contains the number of original measurements that this aggregated measurement is based on.</summary>
			/// <remarks>Integer</remarks>
			public const ushort AggregatedMeasurementCount = 101;

			/// <summary>Supplier</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Supplier = 801;

			/// <summary>Variant</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Variant = 802;

			/// <summary>Production date</summary>
			/// <remarks>Date</remarks>
			public const ushort ProductionDate = 803;

			/// <summary>Shift</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Shift = 850;

			/// <summary>Grade</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Grade = 851;

			/// <summary>Long-term measurement</summary>
			/// <remarks>Catalog</remarks>
			public const ushort LongTermMeasurement = 852;

			/// <summary>Procedure</summary>
			/// <remarks>Integer</remarks>
			public const ushort Procedure = 10020;

			/// <summary>Incremental part number from CALYPSO / CALIGO</summary>
			/// <remarks>Integer</remarks>
			public const ushort PartNumberIncremental = 10096;

			#endregion

			#region properties

			/// <summary>
			/// Collection of all measurement attributes specific for aggregated measurements
			/// </summary>
			public static ushort[] AggregatedMeasurementKeys => new[]
			{
				AggregationJobUuid,
				AggregationInterval,
				AggregatedMeasurementCount
			};

			#endregion
		}

		#endregion

		#region class Part

		/// <summary>Well known keys for accessing part attributes.</summary>
		public static class Part
		{
			#region constants

			/// <summary>Part number</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Number = 1001;

			/// <summary>Part description</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Description = 1002;

			/// <summary>Part abbreviation</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Abbreviation = 1003;

			/// <summary>Drawing status</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort DrawingStatus = 1004;

			/// <summary>Line</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Line = 1008;

			/// <summary>Documented flag</summary>
			/// <remarks>Catalog</remarks>
			public const ushort ControlItem = 1010;

			/// <summary>Model</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort VariantOfLine = 1011;

			/// <summary>Drawing number</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort DrawingNumber = 1041;

			/// <summary>Drawing name</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort DrawingName = 1046;

			/// <summary>Operation</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Operation = 1086;

			/// <summary>Organization</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Organisation = 1100;

			/// <summary>Division of company</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Division = 1101;

			/// <summary>Cost center</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort CostCenter = 1103;

			/// <summary>Inspection type</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort InspectionType = 1209;

			/// <summary>Company</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Company = 1301;

			/// <summary>Production plant</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Plant = 1303;

			/// <summary>Name of inspection plan</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort InspectionPlanName = 1342;

			/// <summary>Date of creation</summary>
			/// <remarks>Date</remarks>
			public const ushort CreatedDate = 1343;

			/// <summary>Creator</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort CreatedBy = 1344;

			/// <summary>Date of changes</summary>
			/// <remarks>Date</remarks>
			public const ushort ChangedDate = 1508;

			/// <summary>Person that changed the part</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort ChangedBy = 1509;

			/// <summary>Comment</summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort Comment = 1900;

			/// <summary>
			/// The uri that can be used to create an interactive hyperlink that calls another application. This is used by PiWeb Monitor 
			/// for example to switch back to the measuring application (Calypso, Caligo etc.) when clicking on a characteristic or part.
			/// </summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort CallbackUri = 11098;

			/// <summary>
			/// This is a descriptive text for the <see cref="CallbackUri"/>.
			/// </summary>
			/// <remarks>AlphaNumeric</remarks>
			public const ushort CallbackUriText = 11097;

			#endregion
		}

		#endregion

		#region class Value

		/// <summary>Well known keys for accessing value attributes.</summary>
		public static class Value
		{
			#region constants

			/// <summary>Measured value</summary>
			/// <remarks>Float</remarks>
			public const ushort MeasuredValue = 1;

			/// <summary>Key for a measurement value attribute that contains the minimum (calculated by an aggregation job for example).</summary>
			/// <remarks>Float</remarks>
			public const ushort AggregatedMinimum = 21000;

			/// <summary>Key for a measurement value attribute that contains the maximum (calculated by an aggregation job for example).</summary>
			/// <remarks>Float</remarks>
			public const ushort AggregatedMaximum = 21001;

			/// <summary>Key for a measurement value attribute that contains the range (calculated by an aggregation job for example).</summary>
			/// <remarks>Float</remarks>
			public const ushort AggregatedRange = 21002;

			/// <summary>Key for a measurement value attribute that contains the mean value (calculated by an aggregation job for example).</summary>
			/// <remarks>Float</remarks>
			public const ushort AggregatedMean = 21003;

			/// <summary>Key for a measurement value attribute that contains the sigma (calculated by an aggregation job for example).</summary>
			/// <remarks>Float</remarks>
			public const ushort AggregatedSigma = 21004;

			/// <summary>Key for a measurement value attribute that contains the median (calculated by an aggregation job for example).</summary>
			/// <remarks>Float</remarks>
			public const ushort AggregatedMedian = 21005;

			/// <summary>Key for a measurement value attribute that contains the lower quartile (0.25 quantile) (calculated by an aggregation job for example).</summary>
			/// <remarks>Float</remarks>
			public const ushort AggregatedLowerQuartile = 21006;

			/// <summary>Key for a measurement value attribute that contains the upper quartile (0.75 quantile) (calculated by an aggregation job for example).</summary>
			/// <remarks>Float</remarks>
			public const ushort AggregatedUpperQuartile = 21007;

			/// <summary>Key for a measurement value attribute that contains the cp value (calculated by an aggregation job for example).</summary>
			/// <remarks>Float</remarks>
			public const ushort AggregatedCp = 21008;

			/// <summary>Key for a measurement value attribute that contains the cpk value (calculated by an aggregation job for example).</summary>
			/// <remarks>Float</remarks>
			public const ushort AggregatedCpk = 21009;

			/// <summary>Key for a measurement value attribute that contains the number of values (calculated by an aggregation job for example).</summary>
			/// <remarks>Integer</remarks>
			public const ushort AggregatedValueCount = 21010;

			/// <summary>Key for a measurement value attribute that contains the number of characterteristics within yellow range (calculated by an aggregation job for example).</summary>
			/// <remarks>Integer</remarks>
			public const ushort AggregatedYellowRange = 21011;

			/// <summary>Key for a measurement value attribute that contains the number of characterteristics within red range (calculated by an aggregation job for example).</summary>
			/// <remarks>Integer</remarks>
			public const ushort AggregatedRedRange = 21012;

			#endregion

			#region properties

			/// <summary>
			/// Collection of all measurement value attributes specific for aggregated measurements
			/// </summary>
			public static ushort[] AggregatedValueKeys => new[]
			{
				AggregatedMinimum,
				AggregatedMaximum,
				AggregatedRange,
				AggregatedMean,
				AggregatedSigma,
				AggregatedMedian,
				AggregatedLowerQuartile,
				AggregatedUpperQuartile,
				AggregatedCp,
				AggregatedCpk,
				AggregatedValueCount,
				AggregatedYellowRange,
				AggregatedRedRange
			};

			#endregion
		}

		#endregion
	}
}