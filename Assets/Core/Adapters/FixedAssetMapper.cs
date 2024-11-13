/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Assets.Core.dll                    Pattern   : Mapper                                  *
*  Type     : FixedAssetMapper                           License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data mapping services for FixedAsset instances.                                       *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Documents.Services;
using Empiria.History.Services;

using Empiria.StateEnums;

namespace Empiria.Assets.Adapters {

  /// <summary>Provides data mapping services for FixedAsset instances.</summary>
  static internal class FixedAssetMapper {

    static internal FixedAssetHolderDto Map(FixedAsset fixedAsset) {
      return new FixedAssetHolderDto {
        FixedAsset = MapFixedAsset(fixedAsset),
        Transactions = new FixedList<NamedEntityDto>(),
        Documents = DocumentServices.GetEntityDocuments(fixedAsset),
        History = HistoryServices.GetEntityHistory(fixedAsset),
        Actions = MapActions()
      };
    }


    static internal FixedAssetDto MapFixedAsset(FixedAsset fixedAsset) {
      return new FixedAssetDto {
        UID = fixedAsset.UID,
        FixedAssetType = fixedAsset.FixedAssetType.MapToNamedEntity(),
        InventoryNo = fixedAsset.InventoryNo,
        Label = fixedAsset.Label,
        Name = fixedAsset.Name,
        Description = fixedAsset.Description,
        Brand = fixedAsset.Brand,
        Model = fixedAsset.Model,
        Year = fixedAsset.Year,
        Location = fixedAsset.Location,
        CustodianOrgUnit = fixedAsset.CustodianOrgUnit.MapToNamedEntity(),
        CustodianPerson = fixedAsset.CustodianPerson.MapToNamedEntity(),
        StartDate = fixedAsset.StartDate,
        EndDate = fixedAsset.EndDate,
        Status = fixedAsset.Status.MapToDto()
      };
    }


    static internal FixedList<FixedAssetDescriptor> Map(FixedList<FixedAsset> fixedAssets) {
      return fixedAssets.Select(fixedAsset => MapToDescriptor(fixedAsset))
                       .ToFixedList();
    }

    #region Helpers

    static private BaseActions MapActions() {
      return new BaseActions {
        CanEditDocuments = true
      };
    }


    static private FixedAssetDescriptor MapToDescriptor(FixedAsset fixedAsset) {
      return new FixedAssetDescriptor {
        UID = fixedAsset.UID,
        FixedAssetTypeName = fixedAsset.FixedAssetType.Name,
        InventoryNo = fixedAsset.InventoryNo,
        Label = fixedAsset.Label,
        Name = fixedAsset.Name,
        Description = fixedAsset.Description,
        Location = fixedAsset.Location,
        Condition = fixedAsset.Condition,
        CustodianOrgUnitName = fixedAsset.CustodianOrgUnit.FullName,
        StartDate = fixedAsset.StartDate,
        EndDate = fixedAsset.EndDate,
        StatusName = fixedAsset.Status.GetName()
      };
    }

    #endregion Helpers

  }  // class FixedAssetMapper

}  // namespace Empiria.Assets.Adapters
