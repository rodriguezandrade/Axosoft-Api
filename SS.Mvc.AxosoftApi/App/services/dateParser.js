var dateOnlyRegex = /^(\d{4})-(\d{2})-(\d{2})$/,
    isoDateRegex = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2})(\.\d+)?Z$/i;

export class DateParser {
    static get $inject() { return [] }
    static get name() { return 'dateParser' }

    constructor() {
    }

    parse(value) {
		if (typeof value === "string") {
			var m = dateOnlyRegex.exec(value);
			if (m) {
				return new Date(m[1], m[2] - 1, m[3], 0, 0, 0);
			}
			m = isoDateRegex.exec(value);
			if (m) {
				//return new Date(m[1], m[2] - 1, m[3], m[4], m[5]);
				return new Date(value);
			}
		}
			
		if (typeof value === "number") { 
			return new Date(value);
		}

		return value;
	}
}