/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Fixed Assets                               Component : Test cases                              *
*  Assembly : Empiria.Assets.Tests.dll                   Pattern   : Unit tests                              *
*  Type     : FixedAssetTests                            License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Unit tests for FixedAsset instances.                                                           *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Xunit;

using Empiria.Assets;

namespace Empiria.Tests.Assets {

  /// <summary>Unit tests for FixedAsset instances.</summary>
  public class FixedAssetTests {

    #region Facts


    [Fact]
    public void Should_Read_All_Fixed_Assets() {
      var sut = BaseObject.GetList<FixedAsset>();

      Assert.NotNull(sut);
      Assert.NotEmpty(sut);
    }


    [Fact]
    public void Should_Read_Empty_FixedAsset() {
      var sut = FixedAsset.Empty;

      Assert.NotNull(sut);
    }

    #endregion Facts

  }  // class FixedAssetTests

}  // namespace Empiria.Tests.Assets
