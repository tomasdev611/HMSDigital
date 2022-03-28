export function exportFile(data: string, filename: string, filetype: string) {
  if (!filetype) {
    filetype = `csv`;
  }
  if (!filename) {
    filename = `export`;
  }
  if (data) {
    const link = document.createElement('a');
    link.setAttribute('href', `data:attachment/${filetype};charset=utf-8,${encodeURI(data)}`);
    link.setAttribute('type', 'hidden');
    link.setAttribute('download', `${filename}.${filetype}`);
    link.setAttribute('target', '_blank');
    document.body.appendChild(link);
    link.click();
    link.remove();
  }
}
