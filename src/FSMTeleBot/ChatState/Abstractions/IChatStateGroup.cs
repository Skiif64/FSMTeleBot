﻿using System.Collections.Immutable;

namespace FSMTeleBot.ChatState.Abstractions;

public interface IChatStateGroup
{
    ImmutableArray<IChatState> States { get; }
    IChatState this[int index] { get; }

    Task<IChatState> Next(IChatContext context, CancellationToken cancellationToken = default);
}