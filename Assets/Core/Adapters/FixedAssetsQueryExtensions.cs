/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Adapters Layer                          *
*  Assembly : Empiria.Assets.Core.dll                    Pattern   : Type Extensions                         *
*  Type     : FixedAssetsQueryExtensions                 License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Extension methods for FixedAssetsQuery type.                                                   *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Parties;
using Empiria.StateEnums;

namespace Empiria.Assets.Adapters {

  /// <summary>Extension methods for FixedAssetsQuery type.</summary>
  static internal class FixedAssetsQueryExtensions {

    #region Extension Methods

    static internal void EnsureIsValid(this FixedAssetsQuery query) {
      // no - op
    }


    static internal string MapToFilterString(this FixedAssetsQuery query) {

      string custodianOrgUnitFilter = BuildCustodianOrgUnitFilter(query.CustodianOrgUnitUID);

      string statusFilter = BuildStatusFilter(query.Status);

      string inventoryNoFilter = BuildInventoryNoFilter(query.InventoryNo);

      string fixedAssetTypeFilter = BuildFixedAssetTypeFilter(query.FixedAssetTypeUID);

      string keywordsFilter = BuildKeywordsFilter(query.Keywords);


      var filter = new Filter(custodianOrgUnitFilter);

      filter.AppendAnd(statusFilter);
      filter.AppendAnd(inventoryNoFilter);
      filter.AppendAnd(fixedAssetTypeFilter);
      filter.AppendAnd(keywordsFilter);

      return filter.ToString();
    }


    static internal string MapToSortString(this FixedAssetsQuery query) {

      if (query.OrderBy.Length != 0) {
        return query.OrderBy;
      }

      return "FXD_ASST_INVENTORY_NO";

    }

    #endregion Extension Methods

    #region Helpers

    static private string BuildCustodianOrgUnitFilter(string custodianOrgUnitUID) {
      if (custodianOrgUnitUID == string.Empty) {
        return string.Empty;
      }

      var custodianOrgUnit = OrganizationalUnit.Parse(custodianOrgUnitUID);

      return $"FXD_ASST_CUSTODIAN_ORG_UNIT_ID = {custodianOrgUnit.Id}";
    }


    static private string BuildFixedAssetTypeFilter(string fixedAssetTypeUID) {
      if (fixedAssetTypeUID == string.Empty) {
        return string.Empty;
      }

      var fixedAssetType = FixedAssetType.Parse(fixedAssetTypeUID);

      return $"FXD_ASST_TYPE_ID = {fixedAssetType.Id}";
    }


    static private string BuildInventoryNoFilter(string inventoryNo) {
      if (inventoryNo == string.Empty) {
        return string.Empty;
      }

      return $"FXD_ASST_INVENTORY_NO LIKE '%{inventoryNo}%'";
    }


    static private string BuildKeywordsFilter(string keywords) {
      if (keywords == string.Empty) {
        return string.Empty;
      }

      return SearchExpression.ParseAndLike("FXD_ASST_KEYWORDS", keywords);
    }


    static private string BuildStatusFilter(EntityStatus status) {
      if (status == EntityStatus.All) {
        return "FXD_ASST_STATUS <> 'X' ";
      }

      return $"FXD_ASST_STATUS = '{(char) status}'";
    }

    #endregion Helpers

  }  // class FixedAssetsQueryExtensions

} // namespace Empiria.Assets.Adapters
