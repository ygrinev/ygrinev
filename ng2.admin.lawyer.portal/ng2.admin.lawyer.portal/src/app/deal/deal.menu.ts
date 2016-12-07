export const DEAL_MENU = [
    {
        path: 'deal',
        children: [
            {
                path: 'dealhistory',
                data: {
                    menu: {
                        title: 'Deal History',
                        icon: 'ion-ios-list-outline',
                        selected: false,
                        expanded: false,
                        order: 10,
                    }
                },
            },
            {
                path: 'notes',
                data: {
                    menu: {
                        title: 'Notes',
                        icon: 'ion-compose',
                        selected: false,
                        expanded: false,
                        order: 100,
                    }
                },
            },
            {
                path: 'documents',
                data: {
                    menu: {
                        title: 'Documents',
                        icon: 'ion-ios-folder-outline',
                        selected: false,
                        expanded: false,
                        order: 200,
                    }
                },
            },
            {
                path: 'srot',
                data: {
                    menu: {
                        title: 'SROT',
                        icon: 'ion-clipboard',
                        selected: false,
                        expanded: false,
                        order: 300,
                    }
                },
            },
            {
                path: 'pif',
                data: {
                    menu: {
                        title: 'PIF',
                        icon: 'ion-information',
                        selected: false,
                        expanded: false,
                        order: 400,
                    }
                },
            }
        ]
    }
];
