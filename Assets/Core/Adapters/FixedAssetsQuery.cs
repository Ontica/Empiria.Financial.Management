/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Assets.Core.dll                    Pattern   : Query DTO                               *
*  Type     : FixedAssetsQuery                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Query data transfer object used to search fixed assets.                                        *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.StateEnums;

namespace Empiria.Assets.Adapters {

  /// <summary>Query data transfer object used to search fixed assets.</summary>
  public class FixedAssetsQuery {

    public string InventoryNo {
      get; set;
    } = string.Empty;


    public string FixedAssetTypeUID {
      get; set;
    } = string.Empty;


    public string CustodianOrgUnitUID {
      get; set;
    } = string.Empty;


    public string Keywords {
      get; set;
    } = string.Empty;


    public EntityStatus Status {
      get; set;
    } = EntityStatus.All;


    public string OrderBy {
      get; set;
    } = string.Empty;

  }  // class FixedAssetsQuery

} // namespace Empiria.Assets.Adapters
