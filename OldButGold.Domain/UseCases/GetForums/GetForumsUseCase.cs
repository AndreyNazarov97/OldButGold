﻿using MediatR;
using Forum = OldButGold.Domain.Models.Forum;

namespace OldButGold.Domain.UseCases.GetForums
{
    internal class GetForumsUseCase : IRequestHandler<GetForumsQuery ,IEnumerable<Forum>>
    {
        private readonly IGetForumsStorage storage;

        public GetForumsUseCase(
            IGetForumsStorage storage)
        {
            this.storage = storage;
        }

        public  Task<IEnumerable<Forum>> Handle(GetForumsQuery query ,CancellationToken cancellationToken)
        {
            return storage.GetForums(cancellationToken);

        }

    }
}
