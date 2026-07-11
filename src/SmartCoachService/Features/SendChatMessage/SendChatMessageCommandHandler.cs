//using System.Net.Sockets;
//using System.Runtime.Intrinsics.Arm;
//using Azure;
//using MediatR;
//using Microsoft.AspNetCore.Http.HttpResults;
//using Microsoft.Identity.Client;
//using SmartCoachService.Clients.FCE;
//using SmartCoachService.Clients.Progress;
//using SmartCoachService.Common;
//using SmartCoachService.Entities;
//using SmartCoachService.Entities.enums;
//using SmartCoachService.Persistence.Repositories.ChatMessageRepository;
//using SmartCoachService.Persistence.Repositories.ChatSessionRepository;
//using SmartCoachService.Persistence.Repositories.UnitOfWork;
//using SmartCoachService.Services.AI;
//using SmartCoachService.Services.Cache;
//using SmartCoachService.Services.ChatRateLimit;
//using SmartCoachService.Services.CurrentUser;
//using SmartCoachService.Services.Prompt;
//using static System.Collections.Specialized.BitVector32;
//using static System.Runtime.InteropServices.JavaScript.JSType;

//namespace SmartCoachService.Features.SendChatMessage
//{
//    public sealed class SendChatMessageCommandHandler(
//                      IChatSessionRepository _chatSessionRepo,
//                      IChatMessageRepository _chatMessageRepo,
//                      IRecommendationCacheService _recommendationCacheService,
//                      IFceClient _fceClient,
//                      IProgressClient _progressClient,
//                      IPromptBuilder _promptBuilder,
//                      IAIService _aiService,
//                      ICurrentUserService _currentUser,
//                      IChatRateLimitService _rateLimitService,
//                      IUnitOfWork _unitOfWork)
//        : IRequestHandler<SendChatMessageCommand, RequestResult<SendChatMessageResponse>>
//    {
//        public async Task<RequestResult<SendChatMessageResponse>> Handle(SendChatMessageCommand request,
//            CancellationToken cancellationToken)
//        {

//            //1 - Check Rate Limit
//            //2 - Get / Create Session
//            //3 - Get Recommendation Cache
//            //4 - Cache Miss
//            //      ↓
//            //   FCE
//            //      ↓
//            //   Progress
//            //      ↓
//            //   Save Cache
//            //5 - Build Prompt
//            //6 - Call AI
//            //7 - Create UserMessage
//            //8 - Create AIMessage
//            //9 - Add Messages
//            //10 - Increment Rate Limit
//            //11 - SaveChanges
//            //12 - Return Response

//            //----------------------------------------------------
//            // Get Current User
//            //----------------------------------------------------

//            var userId = _currentUser.UserId;

//            //----------------------------------------------------
//            // Check Free Tier Limit
//            //----------------------------------------------------

//            var canSend = await _rateLimitService.CanSendMessageAsync(userId, cancellationToken);

//            if (!canSend)
//            {
//                return RequestResult<SendChatMessageResponse>.Failure(
//                    "PERM_PREMIUM_REQUIRED",
//                    statusCode: StatusCodes.Status403Forbidden);
//            }

//            //----------------------------------------------------
//            // Load Session
//            //----------------------------------------------------

//            ChatSession session;

//            if (request.SessionId is null)
//            {
//                session = ChatSession.Create(userId);

//                _chatSessionRepo.Add(session);
//            }
//            else
//            {
//                session = await _chatSessionRepo.GetByIdAsync(request.SessionId.Value,
//                                                              cancellationToken);

//                if (session is null || session.UserId != userId)
//                {
//                    return RequestResult<SendChatMessageResponse>.Failure(
//                        "RES_SESSION_NOT_FOUND",
//                        statusCode: StatusCodes.Status404NotFound);
//                }
//            }

//            //----------------------------------------------------
//            // Load Recommendation Cache
//            //----------------------------------------------------

//            var userContext = await _recommendationCacheService.GetUserContextAsync(
//                                        cancellationToken);

//            //----------------------------------------------------
//            // Cache Miss
//            //----------------------------------------------------

//            if (string.IsNullOrWhiteSpace(userContext))
//            {
//                var biometrics =
//                    await _fceClient.GetUserContextAsync(
//                        userId,
//                        cancellationToken);

//                var progress =
//                    await _progressClient.GetUserProgressAsync(
//                        userId,
//                        cancellationToken);

//                //------------------------------------------------
//                // Temporary
//                //------------------------------------------------

//                userContext =
//                             $$"""
//                             Weight: {{biometrics?.Weight}}
                             
//                             Height: {{biometrics?.Height}}
                             
//                             Birth Date: {{biometrics?.BirthDate}}
                             
//                             Gender: {{biometrics?.Gender}}
                             
//                             Goal: {{biometrics?.Goal}}
                             
//                             Activity Level: {{biometrics?.ActivityLevel}}
                             
//                             Completed Workouts: {{progress?.CompletedWorkouts}}
//                             """;

//                //----------------------------------------------------
//                // Save Recommendation Cache
//                //----------------------------------------------------

//                await _recommendationCacheService.SetUserContextAsync(
//                    userContext,
//                    string.Empty,
//                    cancellationToken);

//                //----------------------------------------------------
//                // Build Prompt
//                //----------------------------------------------------

//                var prompt = _promptBuilder.Build(
//                    request.Message,
//                    userContext);

//                //----------------------------------------------------
//                // Send Prompt To AI
//                //----------------------------------------------------

//                var aiResponse = await _aiService.GenerateReplyAsync(
//                    new AIRequest(prompt, userContext),
//                    cancellationToken);

//                //----------------------------------------------------
//                // Save User Message
//                //----------------------------------------------------

//                var userMessage = ChatMessage.Create(
//                    session.Id,
//                    SenderType.User,
//                    request.Message);

//                var aiMessage = ChatMessage.Create(
//                    session.Id,
//                    SenderType.AI,
//                    aiResponse.Reply);

//                _chatMessageRepo.Add(userMessage);
//                _chatMessageRepo.Add(aiMessage);

//                //----------------------------------------------------
//                // Save AI Message
//                //----------------------------------------------------

//                var assistantMessage = ChatMessage.Create(
//                    session.Id,
//                    SenderType.AI,
//                    aiResponse.Reply);

//                _chatMessageRepo.Add(assistantMessage);

//                //----------------------------------------------------
//                // Increment Rate Limit
//                //----------------------------------------------------

//                await _rateLimitService.IncrementAsync(
//                    userId,
//                    cancellationToken);

//                //----------------------------------------------------
//                // Save Changes
//                //----------------------------------------------------

//                await _unitOfWork.SaveChangesAsync(
//                    cancellationToken);

//                //----------------------------------------------------
//                // Return Response
//                //----------------------------------------------------

//                var response = new SendChatMessageResponse(
//                    session.Id,
//                    aiResponse.Reply,
//                    aiResponse.FollowUpSuggestions.ToList());

//                return RequestResult<SendChatMessageResponse>.Success(response);
//            }

//        }
//    }
//}
