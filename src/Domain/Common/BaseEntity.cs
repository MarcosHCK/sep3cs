/* Copyright (c) 2023-2025
 * This file is part of sep3cs.
 *
 * sep3cs is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * sep3cs is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with sep3cs. If not, see <http://www.gnu.org/licenses/>.
 */
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataClash.Domain.Common
{
  public abstract class BaseEntity
    {
      [Key]
      public long Id { get; set; }

      [NotMapped]
      public IReadOnlyCollection<BaseEvent> DomainEvents => _domainEvents.AsReadOnly ();
      private readonly List<BaseEvent> _domainEvents = new ();

      public void AddDomainEvent (BaseEvent domainEvent) => _domainEvents.Add (domainEvent);
      public void RemoveDomainEvent (BaseEvent domainEvent) => _domainEvents.Remove (domainEvent);
      public void ClearDomainEvents () => _domainEvents.Clear ();
    }
}
