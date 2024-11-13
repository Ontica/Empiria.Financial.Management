/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Assets.Core.dll                    Pattern   : Data Transfer Object                    *
*  Type     : FixedAssetDto, FixedAssetHolderDto         License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Data transfer object used to return fixed asset data.                                          *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System;

using Empiria.Documents.Services.Adapters;
using Empiria.History.Services.Adapters;

namespace Empiria.Assets.Adapters {

  public class FixedAssetHolderDto {

    public FixedAssetDto FixedAsset {
      get; internal set;
    }

    public FixedList<NamedEntityDto> Transactions {
      get; internal set;
    }

    public FixedList<DocumentDto> Documents {
      get; internal set;
    }

    public FixedList<HistoryDto> History {
      get; internal set;
    }

    public BaseActions Actions {
      get; internal set;
    }

  }  // class FixedAssetHolderDto



  /// <summary>Data transfer object used to return fixed asset data.</summary>
  public class FixedAssetDto {

    public string UID {
      get; internal set;
    }

    public NamedEntityDto FixedAssetType {
      get; internal set;
    }

    public string InventoryNo {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public string Brand {
      get; internal set;
    }

    public string Model {
      get; internal set;
    }

    public int Year {
      get; internal set;
    }

    public string Label {
      get; internal set;
    }

    public NamedEntityDto CustodianOrgUnit {
      get; internal set;
    }

    public NamedEntityDto CustodianPerson {
      get; internal set;
    }

    public string Location {
      get; internal set;
    }

    public DateTime StartDate {
      get; internal set;
    }

    public DateTime EndDate {
      get; internal set;
    }

    public NamedEntityDto Status {
      get; internal set;
    }

  }  // class FixedAssetDto



  /// <summary>Output Dto used to return minimal data for a fixed asset.</summary>
  public class FixedAssetDescriptor {

    public string UID {
      get; internal set;
    }

    public string FixedAssetTypeName {
      get; internal set;
    }

    public string InventoryNo {
      get; internal set;
    }

    public string Name {
      get; internal set;
    }

    public string Description {
      get; internal set;
    }

    public string Label {
      get; internal set;
    }

    public string CustodianOrgUnitName {
      get; internal set;
    }

    public string Location {
      get; internal set;
    }

    public string Condition {
      get; internal set;
    }

    public DateTime StartDate {
      get; internal set;
    }

    public DateTime EndDate {
      get; internal set;
    }

    public string StatusName {
      get; internal set;
    }

  } // class FixedAssetDescriptor

}  // namespace Empiria.Assets.Adapters
