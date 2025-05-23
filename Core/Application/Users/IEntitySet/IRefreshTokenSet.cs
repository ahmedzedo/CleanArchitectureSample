﻿using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Domain.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Users.IEntitySet
{
    public interface IRefreshTokenSet : IEntitySet<RefreshToken>
    {
        Task<RefreshToken?> JoinUser(CancellationToken cancellationToken = default);
        Task<RefreshToken?> GetRefreshTokenWithUser(string token,CancellationToken cancellationToken = default);
    }
}
