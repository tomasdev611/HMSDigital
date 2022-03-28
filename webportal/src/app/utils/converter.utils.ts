export function convertToCm(ft, inch) {
  return (ft * 12 + inch) * 2.54;
}
export function convertToFtInch(cm: number) {
  const inch = cm * 0.39370079;
  return {feet: Math.floor(inch / 12), inch: Math.floor(inch % 12)};
}

export function titleCase(str: string) {
  return str
    ? str
        .toLowerCase()
        .split(' ')
        .map((word: string) => {
          return word.charAt(0).toUpperCase() + word.slice(1);
        })
        .join(' ')
    : '';
}
