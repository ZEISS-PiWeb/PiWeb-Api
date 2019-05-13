#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Data
{
	/// <summary>
	/// Static class with well known attribute keys.
	/// </summary>
	public static class WellKnownKeys
	{
		#region Part

		/// <summary>Well known keys for accessing part attributes.</summary>
		public static class Part
		{
			/// <summary>Part number</summary>
			public const ushort Number = 1001;

			/// <summary>Part description</summary>
			public const ushort Description = 1002;

			/// <summary>Part abbreviation</summary>
			public const ushort Abbreviation = 1003;

			/// <summary>Drawing status</summary>
			public const ushort DrawingStatus = 1004;

			/// <summary>Line</summary>
			public const ushort Line = 1008;

			/// <summary>Documented flag</summary>
			public const ushort ControlItem = 1010;

			/// <summary>Model</summary>
			public const ushort VariantOfLine = 1011;

			/// <summary>Drawing number</summary>
			public const ushort DrawingNumber = 1041;

			/// <summary>Drawing name</summary>
			public const ushort DrawingName = 1046;

			/// <summary>Operation</summary>
			public const ushort Operation = 1086;

			/// <summary>Organization</summary>
			public const ushort Organisation = 1100;

			/// <summary>Cost center</summary>
			public const ushort CostCenter = 1103;

			/// <summary>Inspection type</summary>
			public const ushort InspectionType = 1209;

			/// <summary>Production plant</summary>
			public const ushort Plant = 1303;

			/// <summary>
			/// The uri that can be used to create an interactive hyperlink that calls another application. This is used by PiWeb Monitor 
			/// for example to switch back to the measuring application (Calypso, Caligo etc.) when clicking on a characteristic or part.
			/// </summary>
			public const ushort CallbackUri = 11098;

			/// <summary>
			/// This is a descriptive text for the <see cref="CallbackUri"/>.
			/// </summary>
			public const ushort CallbackUriText = 11097;

			#region AUDI-specific

			/// <summary>Abstimmungsdatum</summary>
			public const ushort AdjustmentDate = 1501;

			/// <summary>Eingegeben am</summary>
			public const ushort CreationDate = 1506;

			/// <summary>Eingegeben von</summary>
			public const ushort CreatedBy = 1507;

			/// <summary>Geändert am</summary>
			public const ushort UpdateDate = 1508;

			/// <summary>Geändert von</summary>
			public const ushort UpdatedBy = 1509;

			/// <summary>Organisiationseinheit</summary>
			public const ushort OrganisationalUnit = 1510;

			/// <summary>Straße</summary>
			public const ushort ProductionLine = 1511;

			/// <summary>Verantwortlich</summary>
			public const ushort Responsible = 1512;

			#endregion

			/// <summary>Comment</summary>
			public const ushort Comment = 1900;
		}

		#endregion

		#region Characteristic

		/// <summary>Well known keys for accessing part characteristic.</summary>
		public static class Characteristic
		{
			/// <summary>Characteristic number</summary>
			public const ushort Number = 2001;

			/// <summary>Characteristic description</summary>
			public const ushort Description = 2002;

			/// <summary>Characteristic abbreviation</summary>
			public const ushort Abbreviation = 2003;

			/// <summary>Characteristic direction</summary>
			public const ushort Direction = 2004;

			/// <summary>Characteristic group type</summary>
			public const ushort GroupType = 2008;

			/// <summary>Measurement point role</summary>
			public const ushort MeasurementPointRole = 2343;

			/// <summary>Type of alignment (RPS, 321, ...)</summary>
			public const ushort AlignmentType = 2297;

			/// <summary>Position of a characteristic (left; right)</summary>
			public const ushort Position = 2298;

			/// <summary>Orientation of a characteristic (X; Y; Z; ...)</summary>
			public const ushort Orientation = 2299;

			/// <summary>Documented flag</summary>
			public const ushort ControlItem = 2006;

			/// <summary>Distribution type</summary>
			public const ushort DistributionType = 2011;

			/// <summary>The formula that describes a calculated characteristics</summary>
			public const ushort LogicalOperationString = 2021;

			/// <summary>Number decimal places</summary>
			public const ushort DecimalPlaces = 2022;

			/// <summary>Desired value</summary>
			public const ushort DesiredValue = 2100;

			/// <summary>Nominal value</summary>
			public const ushort NominalValue = 2101;

			/// <summary>Lower specification limit</summary>
			public const ushort LowerSpecificationLimit = 2110;

			/// <summary>Upper specification limit</summary>
			public const ushort UpperSpecificationLimit = 2111;

			/// <summary>Lower tolerance limit</summary>
			public const ushort LowerTolerance = 2112;

			/// <summary>Upper tolerance limit</summary>
			public const ushort UpperTolerance = 2113;

			/// <summary>Lower scrap limit</summary>
			public const ushort LowerScrapLimit = 2114;

			/// <summary>Upper scrap limit</summary>
			public const ushort UpperScrapLimit = 2115;

			/// <summary>Lower natural boundary</summary>
			public const ushort LowerNaturallyBoundary = 2120;

			/// <summary>Upper natural boundary</summary>
			public const ushort UpperNaturallyBoundary = 2121;

			/// <summary>The unit (mm, inch, °, etc.)</summary>
			public const ushort Unit = 2142;

			/// <summary>Name of inspection plan</summary>
			public const ushort NameOfQualityControlPlan = 2342;

			/// <summary>Name of the alignment (Ausrichtung_RPS, ...)</summary>
			public const ushort AlignmentName = 2511;

			/// <summary>Normal vector, x coordinate</summary>
			public const ushort I = 2540;

			/// <summary>Normal vector, y coordinate</summary>
			public const ushort J = 2541;

			/// <summary>Normal vector, z coordinate</summary>
			public const ushort K = 2542;

			/// <summary>Position, x coordinate</summary>
			public const ushort X = 2543;

			/// <summary>Position, y coordinate</summary>
			public const ushort Y = 2544;

			/// <summary>Position, z coordinate</summary>
			public const ushort Z = 2545;

			/// <summary>Layer</summary>
			public const ushort Layer = 2555;

			#region Stamps

			/// <summary>
			/// Stamp text
			/// </summary>
			public const ushort HasStamp = 2808;

			/// <summary>
			/// Stamp text
			/// </summary>
			public const ushort StampCaption = 2809;

			/// <summary>
			/// Stamp position, x coordinate relative to bitmap
			/// </summary>
			public const ushort StampPositionX = 2810;

			/// <summary>
			/// Stamp position, y coordinate relative to bitmap
			/// </summary>
			public const ushort StampPositionY = 2811;

			/// <summary>
			/// Stamp target, x coordinate relative to bitmap
			/// </summary>
			public const ushort StampTargetX = 2812;

			/// <summary>
			/// Stamp target, y coordinate relative to bitmap
			/// </summary>
			public const ushort StampTargetY = 2813;

			/// <summary>
			/// Stamp radius relative to the bitmap width
			/// </summary>
			public const ushort StampRadius = 2814;

			#endregion
			
			#region AUDI-specific

			/// <summary>AUDI: Kategorie (Analysemaß; Bemusterungsmaß; Netzmaß; ...)</summary>
			public const ushort AudiCategory = 2570;

			/// <summary>AUDI: Funktionsgruppe</summary>
			public const ushort AudiFunctionGroup = 2571;

			/// <summary>AUDI: Kennzeichnung von PBMS-Punkten (Wert=1)</summary>
			public const ushort PBMS = 2580;

			/// <summary>AUDI: Kennzeichnung von Inline-Punkten (Wert=1)</summary>
			public const ushort Inline = 2581;

			/// <summary>AUDI: Kennzeichnung von symmetrischen Merkmale (Wert=1)</summary>
			public const ushort Symmetric = 2582;

			/// <summary>AUDI: Kennzeichnung von manueller Toleranzeingabe</summary>
			public const ushort ManualToleranceInput = 2583;

			/// <summary>AUDI: Kennzeichnung von FM relevanten Punkten (Wert=1). FM relevante Merkmale sind 
			/// üblicherweise die Merkmale, aus denen ein Funktionsmaß gebildet wird (z.B. die Einzelpunkte 
			/// einer Distanz).</summary>
			public const ushort FM_Relevant = 2584;

			/// <summary>Auditfunktion</summary>
			public const ushort AuditFunction = 2805;

			/// <summary>Untere Eingriffgrenze</summary>
			public const ushort LowerControlLimit = 8012;

			/// <summary>Obere Eingriffgrenze</summary>
			public const ushort UpperControlLimit = 8013;

			/// <summary>Untere Warngrenze</summary>
			public const ushort LowerWarningLimit = 8014;

			/// <summary>Obere Warngrenze</summary>
			public const ushort UpperWarningLimit = 8015;

			/// <summary>Merkmalsart (variabel; attributiv und attributiv zählend)</summary>
			public const ushort CharacteristicType = 12004;

			/// <summary>Messwertkatalog zur Auswertung attributiver Merkmale</summary>
			public const ushort MeasurementValueCatalog = 12005;

			/// <summary>Messgroesse (fuer Formplot)</summary>
			public const ushort PlotMeasurand = 12009;

			/// <summary>Typ des Merkmals (Circle; Cone; Plane; ...)</summary>
			public const ushort CharacteristicSpecification = 12010;

			/// <summary>Beschreibungstext 1 des Merkmals</summary>
			public const ushort Text1 = 13001;

			/// <summary>Beschreibungstext 2 des Merkmals</summary>
			public const ushort Text2 = 13002;

			/// <summary>Beschreibungstext 3 des Merkmals</summary>
			public const ushort Text3 = 13003;

			/// <summary>Dem Merkmal zugeordnete Kategorie.</summary>
			public const ushort Category = NameOfQualityControlPlan;

			/// <summary>Name des Messelementes des Merkmals</summary>
			public const ushort FeatureName = 12360;

			/// <summary>Typ der Verknüpfung (SYM; DIST; ..) eines Merkmals</summary>
			public const ushort OperationType = 12361;

			/// <summary>Messprogramm</summary>
			public const ushort AudiMeasurementProgram = 13010;

			/// <summary>Schnittebene</summary>
			public const ushort AudiSectionalPlane = 13011;

			/// <summary>FMK-Kennung</summary>
			public const ushort AudiFmk = 13012;

			/// <summary>Tolerenzkette</summary>
			public const ushort AudiToleranceChain = 13013;

			/// <summary>Messprinzip</summary>
			public const ushort AudiMeasuringPrinciple = 13014;

			#endregion

			#region KPI-specific

			// These are sensible values for the new KPI keys;
			// public const ushort GestampKPIWeight = 2830;
			// public const ushort GestampControlPoint = 2831;

			// We currently use these keys however, since these are in use by gestamp. Note how these keys do not fall into characteristic key range
			// and therefore will cause trouble with .dfq files.
			public const ushort GestampControlPoint = 860;
			public const ushort GestampKpiWeight = 964;

			#endregion

			/// <summary>
			/// The uri that can be used to create an interactive hyperlink that calls another application. This is used by PiWeb Monitor 
			/// for example to switch back to the measuring application (Calypso, Caligo etc.) when clicking on a characteristic or part.
			/// </summary>
			public const ushort CallbackUri = 2098;

			/// <summary>
			/// This is a descriptive text for the <see cref="CallbackUri"/>.
			/// </summary>
			public const ushort CallbackUriText = 2097;

			/// <summary>Measured quantity type</summary>
		    public const ushort MeasuredQuantityType = 13266;

			/// <summary>The sample size for a process distribution analysis.</summary>
			public const ushort PdaSampleSize = 8500;

			/// <summary>
			/// Identifying different creation types of multi-samples for process distribution analysis.
			/// </summary>
			public const ushort PdaSampleType = 8501;

			/// <summary>
			/// Identifying the different modes for distribution analysis.
			/// </summary>
			public const ushort DistributionAnalysisMode = 13282;

			#region ControlCharts

			/// <summary>Regelkartenkonfiguration der Lagekarte</summary>
			public const ushort LocationChartConfiguration = 9010;

			/// <summary>Mittellinie der Lagekarte</summary>
			public const ushort LocationChartAverageValue = 9011;

			/// <summary>Untere Eingriffgrenze der Lagekarte</summary>
			public const ushort LocationChartLowerControlLimit = 9012;

			/// <summary>Obere Eingriffgrenze der Lagekarte</summary>
			public const ushort LocationChartUpperControlLimit = 9013;

			/// <summary>Untere Warngrenze der Lagekarte</summary>
			public const ushort LocationChartLowerWarningLimit = 9014;

			/// <summary>Obere Warngrenze der Lagekarte</summary>
			public const ushort LocationChartUpperWarningLimit = 9015;


			/// <summary>Regelkartenkonfiguration der Streuungskarte</summary>
			public const ushort VariationChartConfiguration = 9110;

			/// <summary>Mittellinie der Streuungskarte</summary>
			public const ushort VariationChartAverageValue = 9111;

			/// <summary>Untere Eingriffgrenze der Streuungskarte</summary>
			public const ushort VariationChartLowerControlLimit = 9112;

			/// <summary>Obere Eingriffgrenze der Streuungskarte</summary>
			public const ushort VariationChartUpperControlLimit = 9113;

			/// <summary>Untere Warngrenze der Streuungskarte</summary>
			public const ushort VariationChartLowerWarningLimit = 9114;

			/// <summary>Obere Warngrenze der Streuungskarte</summary>
			public const ushort VariationChartUpperWarningLimit = 9115;

			#endregion
		}

		#endregion

		#region Measurement

		/// <summary>Well known keys for accessing measurement attributes.</summary>
		public static class Measurement
		{
			/// <summary>Time</summary>
			public const ushort Time = 4;

			/// <summary>Event Id</summary>
			public const ushort EventId = 5;

			/// <summary>Batch number</summary>
			public const ushort BatchNumber = 6;

			/// <summary>Inspector name</summary>
			public const ushort InspectorName = 8;

			/// <summary>Comment</summary>
			public const ushort Comment = 9;

			/// <summary>Machine number</summary>
			public const ushort MachineNumber = 10;

			/// <summary>Process Id</summary>
			public const ushort ProcessId = 11;

			/// <summary>Inspection equipment</summary>
			public const ushort InspectionEquipment = 12;

			/// <summary>Process value</summary>
			public const ushort ProcessValue = 13;

			/// <summary>Part Id</summary>
			public const ushort PartsId = 14;

			/// <summary>Inspection type</summary>
			public const ushort InspectionType = 15;

			/// <summary>Inspection number</summary>
			public const ushort ProductionNumber = 16;

			/// <summary>Contract</summary>
			public const ushort Contract = 53;

			/// <summary>Shift</summary>
			public const ushort Shift = 850;

			/// <summary>Incremental part number from CALYPSO / CALIGO</summary>
			public const ushort PartNumberIncremental = 10096;

			/// <summary>Measurement status (approved; blocked; ...)</summary>
			public const ushort MeasurementStatus = 96;

			/// <summary>Measurement change date</summary>
			public const ushort MeasurementChangeDate = 97;

			/// <summary>Measurement changed by</summary>
			public const ushort MeasurementChangedBy = 98;

			/// <summary>Contains the uuid of the aggregation job that created this measurement. Empty if this measurement is not an aggregated measurement.</summary>
			public const ushort AggregationJobUuid = 99;

			/// <summary>Contains the aggregation interval that was used to create this aggregated measurement.</summary>
			public const ushort AggregationInterval = 100;

			/// <summary>Contains the number of original measurements that this aggregated measurement is based on.</summary>
			public const ushort AggregatedMeasurementCount = 101;

			/// <summary>
			/// Collection of all measurement attributes specific for aggregated measurements
			/// </summary>
			public static ushort[] AggregatedMeasurementKeys
			{
				get
				{
					return new[]
					{
						AggregationJobUuid,
						AggregationInterval,
						AggregatedMeasurementCount
					};
				}
			}
		}

		#endregion

		#region Catalog

		/// <summary>Well known keys for accessing value attributes.</summary>
		public static class Catalog
		{
			#region Daimler specific

			/// <summary>Auditkatalog: Funktionsgruppe</summary>
			public const ushort AuditFunctionGroupKey = 4401;

			/// <summary>Auditkatalog: Priorität</summary>
			public const ushort AuditPriorityKey = 4402;

			#endregion

			#region Misc

			/// <summary>Color scheme position</summary>
			public const ushort ColorSchemePositionKey = 2902;

			/// <summary>Status color</summary>
			public const ushort StatusColorKey = 2903;

			/// <summary>Lower class limit</summary>
			public const ushort LowerClassLimitKey = 2135;

			/// <summary>Upper class limit</summary>
			public const ushort UpperClassLimitKey = 2136;

			#endregion

			#region Statistics

			/// <summary>The key for the distribution type.</summary>
			public const ushort DistributionTypeKey = 4403;

			/// <summary>The key for the measured quantity type.</summary>
			public const ushort MeasuredQuantityTypeKey = 13267;

			/// <summary>The description of the pda sample type.</summary>
			public const ushort PdaSampleTypeDescription = 13270;

			/// <summary>The key for the pda sample type.</summary>
			public const ushort PdaSampleTypeKey = 13271;

			/// <summary>The description for the distribution analysis mode.</summary>
			public const ushort DistributionAnalysisModeDescription = 13280;

			/// <summary>The key for the distribution analysis mode.</summary>
			public const ushort DistributionAnalysisModeKey = 13281;

			#endregion

			#region KPI-specific

			// These are sensible values for the new KPI keys;
			//public const ushort GestampKpiWeightKey = 12201;

			// We currently use these keys however, since these are in use by gestamp. Not how these keys do not fall into catalog key range.
			public const ushort GestampKpiWeightKey = 963;

			#endregion
		}

		#endregion

		#region Value

		/// <summary>Well known keys for accessing value attributes.</summary>
		public static class Value
		{
			/// <summary>Measured value</summary>
			public const ushort MeasuredValue = 1;

			/// <summary>Key for a measurement value attribute that contains the minimum (calculated by an aggregation job for example).</summary>
			public const ushort AggregatedMinimum = 21000;

			/// <summary>Key for a measurement value attribute that contains the maximum (calculated by an aggregation job for example).</summary>
			public const ushort AggregatedMaximum = 21001;

			/// <summary>Key for a measurement value attribute that contains the range (calculated by an aggregation job for example).</summary>
			public const ushort AggregatedRange = 21002;

			/// <summary>Key for a measurement value attribute that contains the mean value (calculated by an aggregation job for example).</summary>
			public const ushort AggregatedMean = 21003;

			/// <summary>Key for a measurement value attribute that contains the sigma (calculated by an aggregation job for example).</summary>
			public const ushort AggregatedSigma = 21004;

			/// <summary>Key for a measurement value attribute that contains the median (calculated by an aggregation job for example).</summary>
			public const ushort AggregatedMedian = 21005;

			/// <summary>Key for a measurement value attribute that contains the lower quartile (0.25 quantile) (calculated by an aggregation job for example).</summary>
			public const ushort AggregatedLowerQuartile = 21006;

			/// <summary>Key for a measurement value attribute that contains the upper quartile (0.75 quantile) (calculated by an aggregation job for example).</summary>
			public const ushort AggregatedUpperQuartile = 21007;

			/// <summary>Key for a measurement value attribute that contains the cp value (calculated by an aggregation job for example).</summary>
			public const ushort AggregatedCp = 21008;

			/// <summary>Key for a measurement value attribute that contains the cpk value (calculated by an aggregation job for example).</summary>
			public const ushort AggregatedCpk = 21009;

			/// <summary>Key for a measurement value attribute that contains the number of values (calculated by an aggregation job for example).</summary>
			public const ushort AggregatedValueCount = 21010;

			/// <summary>Key for a measurement value attribute that contains the number of characterteristics within yellow range (calculated by an aggregation job for example).</summary>
			public const ushort AggregatedYellowRange = 21011;

			/// <summary>Key for a measurement value attribute that contains the number of characterteristics within red range (calculated by an aggregation job for example).</summary>
			public const ushort AggregatedRedRange = 21012;

			/// <summary>
			/// Collection of all measurement value attributes specific for aggregated measurements
			/// </summary>
			public static ushort[] AggregatedValueKeys
			{
				get
				{
					return new[]
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
				}
			}
		}

		#endregion
	}
}