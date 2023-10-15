export function parseNumericSuffix(value: string): number {
  if (!value) {
    return 0;
  }
  const numericValue = parseFloat(value);

  if (value.toLowerCase().includes('k')) {
    return numericValue * 1000;
  }
  if (value.toLowerCase().includes('m')) {
    return numericValue * 1000000;
  }
  return numericValue;
}
