/* Empiria Financial *****************************************************************************************
*                                                                                                            *
*  Module   : Payments Management                        Component : Adapters Layer                          *
*  Assembly : Empiria.Payments.Core.dll                  Pattern   : Integration interface                   *
*  Type     : IPaymentsBrokerService                     License   : Please read LICENSE.txt file            *
*                                                                                                            *
*  Summary  : Interface to integrate payments broker service providers with Empiria Payments.                *
*                                                                                                            *
************************* Copyright(c) La Vía Óntica SC, Ontica LLC and contributors. All rights reserved. **/

using System.Threading.Tasks;
using Empiria.Payments.Processor.Adapters;

namespace Empiria.Payments.Processor {

  /// <summary>Interface to integrate payments broker service providers with Empiria Payments.</summary>
  public interface IPaymentsBrokerService {

    Task<BrokerResponseDto> CancelPaymentInstruction(BrokerRequestDto brokerRequest);

    Task<BrokerResponseDto> RequestPaymentStatus(BrokerRequestDto brokerRequest);

    Task<BrokerResponseDto> SendPaymentInstruction(BrokerRequestDto brokerRequest);

  }  // interface IPaymentsBrokerService

}  // namespace Empiria.Payments.Processor
