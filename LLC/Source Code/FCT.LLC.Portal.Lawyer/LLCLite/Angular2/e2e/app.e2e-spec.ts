import { LLCLitePage } from './app.po';

describe('llclite App', function() {
  let page: LLCLitePage;

  beforeEach(() => {
    page = new LLCLitePage();
  });

  it('should display message saying app works', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('app works!');
  });
});
