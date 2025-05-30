﻿using Newtonsoft.Json;

namespace BambuConfigGenerator.Core.Models
{
    public class FilamentModel
    {
        [JsonProperty("activate_air_filtration")]
        public List<string> ActivateAirFiltration { get; set; }

        [JsonProperty("additional_cooling_fan_speed")]
        public List<string> AdditionalCoolingFanSpeed { get; set; }

        [JsonProperty("chamber_temperatures")]
        public List<string> ChamberTemperatures { get; set; }

        [JsonProperty("circle_compensation_speed")]
        public List<string> CircleCompensationSpeed { get; set; }

        [JsonProperty("close_fan_the_first_x_layers")]
        public List<string> CloseFanTheFirstXLayers { get; set; }

        [JsonProperty("compatible_printers")]
        public List<string> CompatiblePrinters { get; set; }

        [JsonProperty("compatible_printers_condition")]
        public string CompatiblePrintersCondition { get; set; }

        [JsonProperty("compatible_prints")]
        public List<object> CompatiblePrints { get; set; }

        [JsonProperty("compatible_prints_condition")]
        public string CompatiblePrintsCondition { get; set; }

        [JsonProperty("complete_print_exhaust_fan_speed")]
        public List<string> CompletePrintExhaustFanSpeed { get; set; }

        [JsonProperty("cool_plate_temp")]
        public List<string> CoolPlateTemp { get; set; }

        [JsonProperty("cool_plate_temp_initial_layer")]
        public List<string> CoolPlateTempInitialLayer { get; set; }

        [JsonProperty("counter_coef_1")]
        public List<string> CounterCoef1 { get; set; }

        [JsonProperty("counter_coef_2")]
        public List<string> CounterCoef2 { get; set; }

        [JsonProperty("counter_coef_3")]
        public List<string> CounterCoef3 { get; set; }

        [JsonProperty("counter_limit_max")]
        public List<string> CounterLimitMax { get; set; }

        [JsonProperty("counter_limit_min")]
        public List<string> CounterLimitMin { get; set; }

        [JsonProperty("default_filament_colour")]
        public List<string> DefaultFilamentColour { get; set; }

        [JsonProperty("diameter_limit")]
        public List<string> DiameterLimit { get; set; }

        [JsonProperty("during_print_exhaust_fan_speed")]
        public List<string> DuringPrintExhaustFanSpeed { get; set; }

        [JsonProperty("enable_overhang_bridge_fan")]
        public List<string> EnableOverhangBridgeFan { get; set; }

        [JsonProperty("enable_pressure_advance")]
        public List<string> EnablePressureAdvance { get; set; }

        [JsonProperty("eng_plate_temp")]
        public List<string> EngPlateTemp { get; set; }

        [JsonProperty("eng_plate_temp_initial_layer")]
        public List<string> EngPlateTempInitialLayer { get; set; }

        [JsonProperty("fan_cooling_layer_time")]
        public List<string> FanCoolingLayerTime { get; set; }

        [JsonProperty("fan_max_speed")]
        public List<string> FanMaxSpeed { get; set; }

        [JsonProperty("fan_min_speed")]
        public List<string> FanMinSpeed { get; set; }

        [JsonProperty("filament_adhesiveness_category")]
        public List<string> FilamentAdhesivenessCategory { get; set; }

        [JsonProperty("filament_change_length")]
        public List<string> FilamentChangeLength { get; set; }

        [JsonProperty("filament_cost")]
        public List<string> FilamentCost { get; set; }

        [JsonProperty("filament_density")]
        public List<string> FilamentDensity { get; set; }

        [JsonProperty("filament_deretraction_speed")]
        public List<string> FilamentDeretractionSpeed { get; set; }

        [JsonProperty("filament_diameter")]
        public List<string> FilamentDiameter { get; set; }

        [JsonProperty("filament_end_gcode")]
        public List<string> FilamentEndGcode { get; set; }

        [JsonProperty("filament_extruder_variant")]
        public List<string> FilamentExtruderVariant { get; set; }

        [JsonProperty("filament_flow_ratio")]
        public List<string> FilamentFlowRatio { get; set; }

        [JsonProperty("filament_id")]
        public string FilamentId { get; set; }

        [JsonProperty("filament_is_support")]
        public List<string> FilamentIsSupport { get; set; }

        [JsonProperty("filament_long_retractions_when_cut")]
        public List<string> FilamentLongRetractionsWhenCut { get; set; }

        [JsonProperty("filament_max_volumetric_speed")]
        public List<string> FilamentMaxVolumetricSpeed { get; set; }

        [JsonProperty("filament_minimal_purge_on_wipe_tower")]
        public List<string> FilamentMinimalPurgeOnWipeTower { get; set; }

        [JsonProperty("filament_notes")]
        public string FilamentNotes { get; set; }

        [JsonProperty("filament_pre_cooling_temperature")]
        public List<string> FilamentPreCoolingTemperature { get; set; }

        [JsonProperty("filament_prime_volume")]
        public List<string> FilamentPrimeVolume { get; set; }

        [JsonProperty("filament_ramming_travel_time")]
        public List<string> FilamentRammingTravelTime { get; set; }

        [JsonProperty("filament_ramming_volumetric_speed")]
        public List<string> FilamentRammingVolumetricSpeed { get; set; }

        [JsonProperty("filament_retract_before_wipe")]
        public List<string> FilamentRetractBeforeWipe { get; set; }

        [JsonProperty("filament_retract_restart_extra")]
        public List<string> FilamentRetractRestartExtra { get; set; }

        [JsonProperty("filament_retract_when_changing_layer")]
        public List<string> FilamentRetractWhenChangingLayer { get; set; }

        [JsonProperty("filament_retraction_distances_when_cut")]
        public List<string> FilamentRetractionDistancesWhenCut { get; set; }

        [JsonProperty("filament_retraction_length")]
        public List<string> FilamentRetractionLength { get; set; }

        [JsonProperty("filament_retraction_minimum_travel")]
        public List<string> FilamentRetractionMinimumTravel { get; set; }

        [JsonProperty("filament_retraction_speed")]
        public List<string> FilamentRetractionSpeed { get; set; }

        [JsonProperty("filament_scarf_gap")]
        public List<string> FilamentScarfGap { get; set; }

        [JsonProperty("filament_scarf_height")]
        public List<string> FilamentScarfHeight { get; set; }

        [JsonProperty("filament_scarf_length")]
        public List<string> FilamentScarfLength { get; set; }

        [JsonProperty("filament_scarf_seam_type")]
        public List<string> FilamentScarfSeamType { get; set; }

        [JsonProperty("filament_settings_id")]
        public List<string> FilamentSettingsId { get; set; }

        [JsonProperty("filament_shrink")]
        public List<string> FilamentShrink { get; set; }

        [JsonProperty("filament_soluble")]
        public List<string> FilamentSoluble { get; set; }

        [JsonProperty("filament_start_gcode")]
        public List<string> FilamentStartGcode { get; set; }

        [JsonProperty("filament_type")]
        public List<string> FilamentType { get; set; }

        [JsonProperty("filament_vendor")]
        public List<string> FilamentVendor { get; set; }

        [JsonProperty("filament_wipe")]
        public List<string> FilamentWipe { get; set; }

        [JsonProperty("filament_wipe_distance")]
        public List<string> FilamentWipeDistance { get; set; }

        [JsonProperty("filament_z_hop")]
        public List<string> FilamentZHop { get; set; }

        [JsonProperty("filament_z_hop_types")]
        public List<string> FilamentZHopTypes { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("full_fan_speed_layer")]
        public List<string> FullFanSpeedLayer { get; set; }

        [JsonProperty("hole_coef_1")]
        public List<string> HoleCoef1 { get; set; }

        [JsonProperty("hole_coef_2")]
        public List<string> HoleCoef2 { get; set; }

        [JsonProperty("hole_coef_3")]
        public List<string> HoleCoef3 { get; set; }

        [JsonProperty("hole_limit_max")]
        public List<string> HoleLimitMax { get; set; }

        [JsonProperty("hole_limit_min")]
        public List<string> HoleLimitMin { get; set; }

        [JsonProperty("hot_plate_temp")]
        public List<string> HotPlateTemp { get; set; }

        [JsonProperty("hot_plate_temp_initial_layer")]
        public List<string> HotPlateTempInitialLayer { get; set; }

        [JsonProperty("impact_strength_z")]
        public List<string> ImpactStrengthZ { get; set; }

        [JsonProperty("inherits")]
        public string Inherits { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nozzle_temperature")]
        public List<string> NozzleTemperature { get; set; }

        [JsonProperty("nozzle_temperature_initial_layer")]
        public List<string> NozzleTemperatureInitialLayer { get; set; }

        [JsonProperty("nozzle_temperature_range_high")]
        public List<string> NozzleTemperatureRangeHigh { get; set; }

        [JsonProperty("nozzle_temperature_range_low")]
        public List<string> NozzleTemperatureRangeLow { get; set; }

        [JsonProperty("overhang_fan_speed")]
        public List<string> OverhangFanSpeed { get; set; }

        [JsonProperty("overhang_fan_threshold")]
        public List<string> OverhangFanThreshold { get; set; }

        [JsonProperty("overhang_threshold_participating_cooling")]
        public List<string> OverhangThresholdParticipatingCooling { get; set; }

        [JsonProperty("pre_start_fan_time")]
        public List<string> PreStartFanTime { get; set; }

        [JsonProperty("pressure_advance")]
        public List<string> PressureAdvance { get; set; }

        [JsonProperty("reduce_fan_stop_start_freq")]
        public List<string> ReduceFanStopStartFreq { get; set; }

        [JsonProperty("required_nozzle_HRC")]
        public List<string> RequiredNozzleHRC { get; set; }

        [JsonProperty("slow_down_for_layer_cooling")]
        public List<string> SlowDownForLayerCooling { get; set; }

        [JsonProperty("slow_down_layer_time")]
        public List<string> SlowDownLayerTime { get; set; }

        [JsonProperty("slow_down_min_speed")]
        public List<string> SlowDownMinSpeed { get; set; }

        [JsonProperty("supertack_plate_temp")]
        public List<string> SupertackPlateTemp { get; set; }

        [JsonProperty("supertack_plate_temp_initial_layer")]
        public List<string> SupertackPlateTempInitialLayer { get; set; }

        [JsonProperty("temperature_vitrification")]
        public List<string> TemperatureVitrification { get; set; }

        [JsonProperty("textured_plate_temp")]
        public List<string> TexturedPlateTemp { get; set; }

        [JsonProperty("textured_plate_temp_initial_layer")]
        public List<string> TexturedPlateTempInitialLayer { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }
    }
}
