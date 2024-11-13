/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Assets Management                          Component : Data Layer                              *
*  Assembly : Empiria.Assets.Core.dll                    Pattern   : Data service                            *
*  Type     : FixedAssetsData                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provides data read and write methods for fixed asset instances.                                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Data;

namespace Empiria.Assets.Data {

  /// <summary>Provides data read and write methods for fixed asset instances.</summary>
  static internal class FixedAssetsData {

    #region Methods

    static internal FixedList<FixedAsset> GetFixedAssets(string filter, string sortBy) {
      var sql = "SELECT * FROM FMS_FIXED_ASSETS";

      if (!string.IsNullOrWhiteSpace(filter)) {
        sql += $" WHERE {filter}";
      }

      if (!string.IsNullOrWhiteSpace(sortBy)) {
        sql += $" ORDER BY {sortBy}";
      }

      var dataOperation = DataOperation.Parse(sql);

      return DataReader.GetFixedList<FixedAsset>(dataOperation);

    }

    #endregion Methods

  }  // class FixedAssetsData

}  // namespace Empiria.Assets.Data
