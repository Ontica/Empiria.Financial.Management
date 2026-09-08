/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Provisions Management                      Component : Use Cases Layer                         *
*  Assembly : Empiria.Provisions.Core.dll                Pattern   : Use Case interactor                     *
*  Type     : ProvisionUseCases                          License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Provision use case interactor.                                                                 *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using Empiria.Services;

using Empiria.Financial;

namespace Empiria.Provisions {

  /// <summary>Provision use case interactor.</summary>
  public class ProvisionUseCases : UseCase {

    #region Constructors and parsers

    protected ProvisionUseCases() {
      // no-op
    }

    static public ProvisionUseCases UseCaseInteractor() {
      return CreateInstance<ProvisionUseCases>();
    }

    #endregion Constructors and parsers

    #region Use cases

    public Provision CreateProvision(IPayableEntity payableEntity) {
      Assertion.Require(payableEntity, nameof(payableEntity));

      var existingProvision = Provision.TryGetFor(payableEntity);

      Assertion.Require(existingProvision == null,
                        $"La {payableEntity.GetEmpiriaType().DisplayName} con número {payableEntity.EntityNo} " +
                        $"ya tiene una provisión registrada.");

      var provision = new Provision(payableEntity);

      provision.Save();

      return provision;
    }

    #endregion Use cases

  }  // class ProvisionUseCases

}  // namespace Empiria.Provisions
