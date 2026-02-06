# Copilot Instructions

## General Guidelines
- In update methods, only reassign properties if the new value is different from the current value to avoid unnecessary assignments.

## Code Style
- For strings, use: `if (property is not null && Property?.Equals(property) is not true) Property = property;`
- For value types, use: `if (value.HasValue && Property != value) Property = value.Value;`